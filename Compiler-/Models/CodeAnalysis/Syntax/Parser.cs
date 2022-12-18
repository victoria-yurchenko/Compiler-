using Compiler.CodeAnalysis.Compilation;
using Compiler.CodeAnalysis.Diagnostics;
using Compiler.CodeAnalysis.Statements;
using Compiler.CodeAnalysis.Syntax.Expressions;
using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler.CodeAnalysis.Text;
using Compiler_.Models.CodeAnalysis.Statements;
using Compiler_.Models.CodeAnalysis.Symbols;
using Compiler_.Models.CodeAnalysis.Syntax.Expressions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using static Compiler.CodeAnalysis.Compilation.CompilationUnit;

namespace Compiler.CodeAnalysis.Syntax
{
    public class Parser
    {
        private readonly DiagnosticList _diagnostic;
        private readonly List<Token> _tokens;

        private int _position;

        public Parser(SourceText text)
        {
            _diagnostic = new DiagnosticList();

            var tokens = new List<Token>();
            var lexer = new Lexer(text);
            Token token;

            do
            {
                token = lexer.GetNextToken();
                if (token.SyntaxKind != SyntaxKind.WhitespaceToken &&
                    token.SyntaxKind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.SyntaxKind != SyntaxKind.EndOfFileToken);


            _tokens = tokens;
            _diagnostic.AddRange(lexer.Diagnostic);
        }

        private Token Current => Peek(0);
        public IEnumerable<Diagnostic> Diagnostic => _diagnostic;

        public CompilationUnit ParseCompilationUnit()
        {
            var programMembers = ParseProgramMembers();
            var endOfFileToken = FindMatch(SyntaxKind.EndOfFileToken);
            return new CompilationUnit(programMembers, endOfFileToken);
        }

        private List<ProgramMember> ParseProgramMembers()
        {
            var programMembers = new List<ProgramMember>();

            while (Current.SyntaxKind != SyntaxKind.EndOfFileToken)
            {
                var startToken = Current;
                var programMember = ParseProgramMember();
                programMembers.Add(programMember);

                if (Current == startToken)
                    GetNextToken();
            }

            return programMembers;
        }

        private ProgramMember ParseProgramMember()
        {
            //if(Current.SyntaxKind == SyntaxKind.FunctionKeyword)
            //    return ParseFunctionDeclaration();

            return ParseGlobalStatement();
        }

        private GlobalStatement ParseGlobalStatement()
        {
            var statement = ParseStatement();
            return new GlobalStatement(statement);
        }

        private ProgramMember ParseFunctionDeclaration()
        {
            var function = FindMatch(SyntaxKind.FunctionKeyword);
            var identifier = FindMatch(SyntaxKind.IdentifierToken);
            var openBracket = FindMatch(SyntaxKind.OpenBracketToken);
            var parameters = ParseParameters();
            var closeBracket = FindMatch(SyntaxKind.CloseBracketToken);
            var type = ParseFunctionParameterType();
            var body = ParseBlockStatement();
            
            return new FunctionDeclaration(function, identifier, openBracket, parameters, type, closeBracket, body);
        }

        private SeparatedSyntaxList<ParameterSyntax> ParseParameters()
        {
            var nodesAndSeparators = new List<Node>();

            while (Current.SyntaxKind != SyntaxKind.CloseBracketToken &&
                   Current.SyntaxKind != SyntaxKind.EndOfFileToken)
            {
                var parameter = ParseParameter();
                nodesAndSeparators.Add(parameter);

                if (Current.SyntaxKind != SyntaxKind.CloseBracketToken)
                {
                    var comma = FindMatch(SyntaxKind.CommaToken);
                    nodesAndSeparators.Add(comma);
                }
            }

            return new SeparatedSyntaxList<ParameterSyntax>(nodesAndSeparators);
        }

        private ParameterSyntax ParseParameter()
        {
            var identifier = FindMatch(SyntaxKind.IdentifierToken);
            var type = ParseType();
            return new ParameterSyntax(identifier, type);
        }

        private Statement ParseStatement()
        {
            switch (Current.SyntaxKind)
            {
                case SyntaxKind.OpenBraceToken:
                    return ParseBlockStatement();

                case SyntaxKind.ConstantKeyword:
                case SyntaxKind.VariableKeyword:
                    return ParseVariableDeclaration();

                case SyntaxKind.IfKeyword:
                    return ParseIfStatement();

                case SyntaxKind.WhileKeyword:
                    return ParseWhileStatement();

                case SyntaxKind.ForKeyword:
                    return ParseForStatement();

                default:
                    return ParseExpressionStatement();
            }
        }

        private Statement ParseForStatement()
        {
            var forKeyword = FindMatch(SyntaxKind.ForKeyword);
            var identifier = FindMatch(SyntaxKind.IdentifierToken);
            var equals = FindMatch(SyntaxKind.EqualsToken);
            var lowerBound = ParseExpression();
            var toKeyword = FindMatch(SyntaxKind.ToKeyword);
            var upperBound = ParseExpression();
            var body = ParseStatement();
            return new For(forKeyword, identifier, equals, lowerBound, toKeyword, upperBound, body);
        }

        private Statement ParseWhileStatement()
        {
            var whileKeyword = FindMatch(SyntaxKind.WhileKeyword);
            var condition = ParseExpression();
            var body = ParseStatement();
            return new While(whileKeyword, condition, body);
        }

        private Statement ParseVariableDeclaration()
        {
            var expectedKeyword = Current.SyntaxKind == SyntaxKind.ConstantKeyword
                                                ? SyntaxKind.ConstantKeyword
                                                : SyntaxKind.VariableKeyword;
            var declarationKeyword = FindMatch(expectedKeyword);
            var identifier = FindMatch(SyntaxKind.IdentifierToken);
            var typeSyntax = ParseFunctionParameterType();
            var equals = FindMatch(SyntaxKind.EqualsToken);
            var initializer = ParseExpression();
            return new VariableDeclaration(declarationKeyword, identifier, typeSyntax, equals, initializer);
        }

        private FunctionParameterTypeSyntax ParseFunctionParameterType()
        {
            if(Current.SyntaxKind != SyntaxKind.ColonToken)
                return null;

            return ParseType();
        }

        private FunctionParameterTypeSyntax ParseType()
        {
            var colonToken = FindMatch(SyntaxKind.ColonToken);
            var identifier = FindMatch(SyntaxKind.IdentifierToken);
            
            return new FunctionParameterTypeSyntax(colonToken, identifier);
        }

        private If ParseIfStatement()
        {
            var ifKeyword = FindMatch(SyntaxKind.IfKeyword);
            var condition = ParseExpression();
            var statement = ParseStatement();
            var elseSyntax = ParseElseSyntax();
            return new If(ifKeyword, condition, statement, elseSyntax);
        }

        private Else ParseElseSyntax()
        {
            if (Current.SyntaxKind != SyntaxKind.ElseKeyword)
                return null;

            var keyword = GetNextToken();
            var statement = ParseStatement();
            return new Else(keyword, statement);
        }

        private ExpressionStatement ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatement(expression);
        }

        private BlockStatement ParseBlockStatement()
        {
            var statements = new List<Statement>();
            var openBraceToken = FindMatch(SyntaxKind.OpenBraceToken);

            while (Current.SyntaxKind != SyntaxKind.EndOfFileToken &&
                   Current.SyntaxKind != SyntaxKind.CloseBraceToken)
            {
                var startToken = Current;

                var statement = ParseStatement();
                statements.Add(statement);

                if (Current == startToken)
                    GetNextToken();
            }

            var closeBraceToken = FindMatch(SyntaxKind.CloseBraceToken);

            return new BlockStatement(openBraceToken, statements, closeBraceToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseVariableAssignmentExpression();
        }

        private ExpressionSyntax ParseVariableAssignmentExpression()
        {
            if (Peek(0).SyntaxKind == SyntaxKind.IdentifierToken &&
                Peek(1).SyntaxKind == SyntaxKind.EqualsToken)
            {
                var identifierToken = GetNextToken();
                var operatorToken = GetNextToken();
                var right = ParseVariableAssignmentExpression();
                return new VariableAssignmentExpression(identifierToken, operatorToken, right);
            }

            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPriority = 0)
        {
            ExpressionSyntax leftOperand;
            var unaryOperatorPriority = Current.SyntaxKind.GetUnaryOperatorPriority();
            if (unaryOperatorPriority != 0 && unaryOperatorPriority >= parentPriority)
            {
                var operatorToken = GetNextToken();
                var operand = ParseBinaryExpression(unaryOperatorPriority);
                leftOperand = new UnaryExpression(operatorToken, operand);
            }
            else
                leftOperand = ParsePrimaryExpression();

            while (true)
            {
                var priority = Current.SyntaxKind.GetBinaryOperatorPriority();
                if (priority == 0 || priority <= parentPriority)
                    break;

                var operatorToken = GetNextToken();
                var rightOperand = ParseBinaryExpression(priority);
                leftOperand = new BinaryExpression(leftOperand, operatorToken, rightOperand);
            }

            return leftOperand;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.SyntaxKind)
            {
                case SyntaxKind.OpenBracketToken:
                    return ParseBracketExpression();

                case SyntaxKind.LiteralToken:
                    return ParseLiteral();

                case SyntaxKind.StringToken:
                    return ParseString();

                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    return ParseBoolean();

                default:
                    return ParseNameOrCallExpression();
            }
        }

        private ExpressionSyntax ParseNameOrCallExpression()
        {
            if (Peek(0).SyntaxKind == SyntaxKind.IdentifierToken &&
                Peek(1).SyntaxKind == SyntaxKind.OpenBracketToken)
                return ParseFunctionCallExpression();

            return ParseNameExpression();
        }

        private ExpressionSyntax ParseFunctionCallExpression()
        {
            var identifier = FindMatch(SyntaxKind.IdentifierToken);
            var openBracket = FindMatch(SyntaxKind.OpenBracketToken);
            var arguments = ParseFunctionArguments();
            var closeBracket = FindMatch(SyntaxKind.CloseBracketToken);
            return new CallExpression(identifier, openBracket, arguments, closeBracket);
        }

        private SeparatedSyntaxList<ExpressionSyntax> ParseFunctionArguments()
        {
            var nodesAndSeparators = new List<Node>();

            while (Current.SyntaxKind != SyntaxKind.CloseBracketToken &&
                   Current.SyntaxKind != SyntaxKind.EndOfFileToken)
            {
                var expression = ParseExpression();
                nodesAndSeparators.Add(expression);

                if(Current.SyntaxKind != SyntaxKind.CloseBracketToken)
                {
                    var comma = FindMatch(SyntaxKind.CommaToken);
                    nodesAndSeparators.Add(comma);
                }
            }

            return new SeparatedSyntaxList<ExpressionSyntax>(nodesAndSeparators);
        }

        private ExpressionSyntax ParseLiteral()
        {
            var numberToken = FindMatch(SyntaxKind.LiteralToken);
            return new LiteralExpression(numberToken);
        }

        private ExpressionSyntax ParseBracketExpression()
        {
            var openBracket = FindMatch(SyntaxKind.OpenBracketToken);
            var expression = ParseExpression();
            var closeBracket = FindMatch(SyntaxKind.CloseBracketToken);
            return new BracketsExpression(openBracket, expression, closeBracket);
        }

        private ExpressionSyntax ParseBoolean()
        {
            var isTrue = Current.SyntaxKind == SyntaxKind.TrueKeyword;
            var keywordToken = isTrue 
                               ? FindMatch(SyntaxKind.TrueKeyword) 
                               : FindMatch(SyntaxKind.FalseKeyword);
            return new LiteralExpression(keywordToken, isTrue);
        }

        private ExpressionSyntax ParseString()
        {
            var stringToken = FindMatch(SyntaxKind.StringToken);
            return new LiteralExpression(stringToken);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var identifierToken = FindMatch(SyntaxKind.IdentifierToken);
            return new VariableNameExpression(identifierToken);
        }

        private Token FindMatch(SyntaxKind kind)
        {
            if (Current.SyntaxKind == kind)
                return GetNextToken();

            _diagnostic.ReportUnexpectedToken(Current.SyntaxKind, kind);
            return new Token(kind, Current.Position, null, null);
        }

        private Token GetNextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private Token Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Count)
                return _tokens[_tokens.Count - 1];

            return _tokens[index];
        }
    }
}
