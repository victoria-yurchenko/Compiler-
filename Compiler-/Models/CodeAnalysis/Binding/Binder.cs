using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler.CodeAnalysis.Binding.Models.BoundOperators;
using Compiler.CodeAnalysis.Binding.Models.Scope;
using Compiler.CodeAnalysis.Compilation;
using Compiler.CodeAnalysis.Diagnostics;
using Compiler.CodeAnalysis.Statements;
using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler_.Models.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler_.Models.CodeAnalysis.Lowering;
using Compiler_.Models.CodeAnalysis.Statements;
using Compiler_.Models.CodeAnalysis.Symbols;
using Compiler_.Models.CodeAnalysis.Syntax.Expressions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Compiler.CodeAnalysis.Compilation.CompilationUnit;

namespace Compiler.CodeAnalysis.Binding
{

    public sealed class Binder
    {
        private readonly DiagnosticList _diagnostic;
        private readonly Function _function;
        

        private BoundScope _boundScope;

        public Binder(BoundScope parent, Function function)
        {
            _diagnostic = new DiagnosticList();
            _boundScope = new BoundScope(parent);
            _function = function;

            if (function != null)
            {
                foreach (var parameter in function.Parameters)
                {
                    _boundScope.TryDeclareVariable(parameter);
                }
            }
        }

        public DiagnosticList Diagnostic => _diagnostic;

        public static BoundProgram BindProgram(BoundGlobalScope globalScope)
        {
            var parentScope = CreateParentScopes(globalScope);

            var functionBodies = new Dictionary<Function, BoundBlockStatement>();
            var diagnostics = new DiagnosticList();

            var scope = globalScope;

            while (scope != null)
            {
                foreach (var function in globalScope.Functions)
                {
                    var binder = new Binder(parentScope, function);
                    var body = binder.BindStatement(function.Declaration.Body);
                    var loweredBody = Rewriter.Rewrite(body);//need to make the bound block statement somehow here
                    functionBodies.Add(function, loweredBody);

                    diagnostics.ToList().AddRange(binder.Diagnostic);
                }

                scope = scope.Previous;
            }

            var statement = Rewriter.Rewrite(new BoundBlockStatement(globalScope.Statements));
            var boundProgram = new BoundProgram(diagnostics, functionBodies, statement);//globalScope, 
            return boundProgram;
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnit syntax)
        {
            var parentScope = CreateParentScopes(previous);
            var binder = new Binder(parentScope, null);

            foreach (var function in syntax.Members.OfType<FunctionDeclaration>())
            {
                binder.BindFunctionDeclaration(function);
            }

            var statements = new List<BoundStatement>();

            foreach (var globalStatement in syntax.Members.OfType<GlobalStatement>())
            {
                var boundStatement = binder.BindStatement(globalStatement.Statement);
                statements.Add(boundStatement);
            }

            //var statement = new BoundBlockStatement(statements);

            var functions = binder._boundScope.GetDeclaredFunctions();
            var variables = binder._boundScope.GetDeclaredVariables();
            var diagnostics = binder.Diagnostic.ToList();

            if (previous != null)
                diagnostics.InsertRange(0, previous.Diagnostics);

            return new BoundGlobalScope(previous, diagnostics, functions, variables, statements);
        }

        private void BindFunctionDeclaration(FunctionDeclaration syntax)
        {
            var parameters = new List<Parameter>();

            var seenParameterNames = new HashSet<string>();

            foreach (var parameterSyntax in syntax.Parameters)
            {
                var parameterName = parameterSyntax.Identifier.Text;
                var parameterType = BindType(parameterSyntax.Type);
                if (!seenParameterNames.Add(parameterName))
                {
                    _diagnostic.ReportParameterAlreadyDeclared(parameterName);
                }
                else
                {
                    var parameter = new Parameter(parameterName, parameterType);
                    parameters.Add(parameter);
                }

            }

            var type = BindType(syntax.FunctionType) ?? SMType.Void;

            if (type != SMType.Void)
                throw new Exception("Unsupported type");

            var function = new Function(syntax.Identifier.Text, parameters, type, syntax);
            if (!_boundScope.TryDeclareFunction(function))
            {
                _diagnostic.ReportFunctionAlreadyExist(function.Name);
                return;
            }
        }

        private SMType BindType(FunctionParameterTypeSyntax syntax)
        {
            if (syntax == null)
                return null;

            var type = LookUp(syntax.Identifier.Text);
            if (type == null)
                _diagnostic.ReportUndefinedType(syntax.Identifier.Text);
            return type;
        }

        private static BoundScope CreateParentScopes(BoundGlobalScope previous)
        {
            var stack = new Stack<BoundGlobalScope>();
            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Previous;
            }

            var parent = GetParent();//getting the root scope here

            while (stack.Count > 0)
            {
                previous = stack.Pop();
                var scope = new BoundScope(parent);

                foreach (var function in previous.Functions)
                {
                    scope.TryDeclareFunction(function);
                }

                foreach (var variable in previous.Variables)
                {
                    scope.TryDeclareVariable(variable);
                }

                parent = scope;
            }
            return parent;
        }

        private static BoundScope GetParent()
        {
            var result = new BoundScope(null);

            foreach (var function in BasicFunctions.GetAll())
                result.TryDeclareFunction(function);

            return result;
        }

        private BoundStatement BindStatement(Statement syntax)
        {
            switch (syntax.SyntaxKind)
            {
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement(syntax as BlockStatement);

                case SyntaxKind.ExpressionStatement:
                    return BindExpressionStatement(syntax as ExpressionStatement);

                case SyntaxKind.VariableDeclaration:
                    return BindVariableDeclation(syntax as VariableDeclaration);

                case SyntaxKind.IfStatement:
                    return BindIfStatement(syntax as If);

                case SyntaxKind.WhileStatement:
                    return BindWhileStatement(syntax as While);

                case SyntaxKind.ForStatement:
                    return BindForStatement(syntax as For);

                default:
                    throw new Exception($"Unexpected syntax {syntax.SyntaxKind}");
            }
        }

        private BoundStatement BindForStatement(For syntax)
        {
            var lowerBound = BindExpression(syntax.LowerBound, SMType.Int);
            var upperBound = BindExpression(syntax.UpperBound, SMType.Int);

            _boundScope = new BoundScope(_boundScope);

            var name = syntax.Identifier.Text;
            var variable = new LocalVariable(name, true, SMType.Int);//new VariableSymbol(name, true, TypeSymbol.Int);
            if (!_boundScope.TryDeclareVariable(variable))
                _diagnostic.ReportVariableAlreadyDeclared(name);

            var body = BindStatement(syntax.Body);

            _boundScope = _boundScope.Parent;

            return new BoundForStatement(variable, lowerBound, upperBound, body);
        }

        private BoundStatement BindWhileStatement(While syntax)
        {
            var condition = BindExpression(syntax.Condition, SMType.Bool);
            var body = BindStatement(syntax.Body);
            return new BoundWhileStatement(condition, body);
        }

        private BoundStatement BindIfStatement(If syntax)
        {
            var condition = BindExpression(syntax.Condition, SMType.Bool);
            var thenStatement = BindStatement(syntax.ThenSyntax);
            var elseStatement = syntax.ElseSyntax == null ? null : BindStatement(syntax.ElseSyntax.ElseStatement);
            return new BoundIfStatement(condition, thenStatement, elseStatement);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, SMType targetType)
        {
            var result = BindExpression(syntax);
            if (result.Type != targetType)
            {
                _diagnostic.ReportCannotConvert(result.Type, targetType);

            }
            return result;
        }

        private BoundStatement BindVariableDeclation(VariableDeclaration syntax)
        {
            var name = syntax.Identifier.Text;
            var isReadOnly = syntax.Keyword.SyntaxKind == SyntaxKind.ConstantKeyword;
            var initializer = BindExpression(syntax.Initializer);
            var variable = _function == null
                           ? (Variable)new GlobalVariable(name, isReadOnly, initializer.Type)
                           : new LocalVariable(name, isReadOnly, initializer.Type);//new VariableSymbol(name, isReadOnly, initializer.Type);

            if (!_boundScope.TryDeclareVariable(variable))
                _diagnostic.ReportVariableAlreadyDeclared(name);

            return new BoundVariableDeclaration(variable, initializer);
        }

        private BoundStatement BindExpressionStatement(ExpressionStatement syntax)
        {
            var expression = BindExpression(syntax.Expression, true);
            return new BoundExpressionStatement(expression);
        }

        private BoundStatement BindBlockStatement(BlockStatement syntax)
        {
            var statements = new List<BoundStatement>();
            _boundScope = new BoundScope(_boundScope);

            foreach (var statementSyntax in syntax.Statements)
            {
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }

            _boundScope = _boundScope.Parent;

            return new BoundBlockStatement(statements.ToList());
        }

        public BoundExpression BindExpression(ExpressionSyntax syntax, bool canBeVoid = false)
        {
            var result = BindExpression(syntax);
            if (!canBeVoid && result.Type != SMType.Void)
            {
                _diagnostic.ReportExpressionMustHaveValue();
                return new BoundErrorExpression();
            }

            return result;
        }

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.SyntaxKind)
            {
                case SyntaxKind.NumberExpression:
                    return BindNumberExpression(syntax as LiteralExpression);

                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression(syntax as BinaryExpression);

                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression(syntax as UnaryExpression);

                case SyntaxKind.BracketsExpression:
                    return BindBracketExpression(syntax as BracketsExpression);

                case SyntaxKind.VariableNameExpression:
                    return BindVariableNameExpression(syntax as VariableNameExpression);

                case SyntaxKind.VariableAssignmentExpression:
                    return BindVariableAssignmentExpression(syntax as VariableAssignmentExpression);

                case SyntaxKind.CallExpression:
                    return BindCallExpression(syntax as CallExpression);

                default:
                    throw new Exception($"Unexpected syntax {syntax.SyntaxKind}");
            }
        }

        private BoundExpression BindCallExpression(CallExpression syntax)
        {
            if (syntax.Arguments.Count == 1 &&
               LookUp(syntax.Identifier.Text) is SMType type)
            {
                return BindConvertion(type, syntax.Arguments[0]);
            }

            var boundArguments = new List<BoundExpression>();

            foreach (var argument in syntax.Arguments)
            {
                var boundArgument = BindExpression(argument);
                boundArguments.Add(boundArgument);
            }

            //var functions = BasicFunctions.GetAll();
            //var function = functions.SingleOrDefault(f => f.Name == syntax.Identifier.Text);
            if (!_boundScope.TryLookUpFunction(syntax.Identifier.Text, out var function))
            {
                _diagnostic.ReportUndefinedFunction(syntax.Identifier.Text);
                return new BoundErrorExpression();
            }

            if (function.Parameters.Count != syntax.Arguments.Count)
            {
                _diagnostic.ReportWrongArgumentCount(function.Name, function.Parameters.Count, syntax.Arguments.Count);
                return new BoundErrorExpression();
            }

            for (int i = 0; i < syntax.Arguments.Count; i++)
            {
                var argument = boundArguments[i];
                var parameter = function.Parameters[i];


                if (argument.Type != parameter.Type)
                {
                    _diagnostic.ReportWrongArgumentType(parameter.Name, parameter.Type, argument.Type);
                    return new BoundErrorExpression();
                }
            }

            return new BoundCallExpression(function, boundArguments);
        }

        private BoundExpression BindConvertion(SMType type, ExpressionSyntax syntax)
        {
            var expression = BindExpression(syntax);
            var convertion = SMConvert.Classify(expression.Type, type);
            if (!convertion.Exists)
            {
                _diagnostic.ReportCannotConvert(expression.Type, type);
                return new BoundErrorExpression();
            }

            return new BoundConvertExpression(type, expression);
        }

        private BoundExpression BindVariableAssignmentExpression(VariableAssignmentExpression syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);

            if (!_boundScope.TryLookUpVariable(name, out var variable))
            {
                _diagnostic.ReportUndefinedVariable(name);
                return new BoundLiteralExpression(0);
            }

            if (variable.IsReadOnly)
                _diagnostic.ReportCannotAssign(name);

            if (variable.Type != boundExpression.Type)
            {
                _diagnostic.ReportCannotConvert(boundExpression.Type, variable.Type);
                return boundExpression;
            }

            return new BoundAssignmentExpression(variable, boundExpression);
        }

        private BoundExpression BindVariableNameExpression(VariableNameExpression syntax)
        {
            var name = syntax.IdentifierToken.Text;

            if (!_boundScope.TryLookUpVariable(name, out var variable))
            {
                _diagnostic.ReportUndefinedVariable(name);
                return new BoundLiteralExpression(0);
            }

            return new BoundVariableExpression(variable);
        }

        private BoundBracketExpression BindBracketExpression(BracketsExpression syntax)
        {
            var result = BindExpression(syntax.Expression);
            return new BoundBracketExpression(result);
        }

        private BoundExpression BindUnaryExpression(UnaryExpression syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.SyntaxKind, boundOperand.Type);
            if (boundOperator == null)
            {
                _diagnostic.ReportUndefinedUnaryOperator(syntax.OperatorToken.Text, boundOperand.Type);
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpression syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);

            if (boundLeft.Kind == BoundNodeKind.BracketExpression)
                boundLeft = (boundLeft as BoundBracketExpression).Expression;

            if (boundLeft.Type == SMType.Error ||
               boundRight.Type == SMType.Error)
                return new BoundErrorExpression();

            var boundOperatorKind = BoundBinaryOperator.Bind(syntax.OperatorToken.SyntaxKind, boundLeft.Type, boundRight.Type);
            if (boundOperatorKind == null)
            {
                _diagnostic.ReportUndefinedBinaryOperator(syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);
                return new BoundErrorExpression();
            }
            return new BoundBinaryExpression(boundLeft, boundOperatorKind, boundRight);
        }

        private BoundExpression BindNumberExpression(LiteralExpression syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private SMType LookUp(string name)
        {
            switch (name)
            {
                case "bool":
                    return SMType.Bool;

                case "int":
                    return SMType.Int;

                case "string":
                    return SMType.String;

                default:
                    return null;
            }
        }
    }
}
