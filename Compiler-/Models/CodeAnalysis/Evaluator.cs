using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler.CodeAnalysis.Statements;
using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler_.Models.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler_.Models.CodeAnalysis.Exceptions;
using Compiler_.Models.CodeAnalysis.Symbols;
using Compiler_.Models.CodeAnalysis.Symbols.Enums;
using Compiler_.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace Compler.CodeAnalysis
{
    public sealed class Evaluator
    {
        private readonly BoundProgram _program;
        private readonly Dictionary<Variable, object> _variables;

        private Random _random;
        private object _lastValue;


        public static List<string> Output { get; set; }

        public Evaluator(BoundProgram program, Dictionary<Variable, object> variables)
        {
            Output = new List<string>();
            _program = program;
            _variables = variables;
            _random = new Random();
        }

        public object Evaluate()
        {
            return EvaluateStatement(_program.Statement);
        }

        private object EvaluateStatement(BoundBlockStatement body)
        {
            var labelIndexes = new Dictionary<BoundLabel, int>();

            for (var i = 0; i < body.Statements.Count; i++)
            {
                if (body.Statements[i] is BoundLabelStatement label)
                    labelIndexes.Add(label.Label, i + 1);
            }

            var index = 0;

            while (index < body.Statements.Count)
            {
                var statement = body.Statements[index];

                switch (statement.Kind)
                {
                    case BoundNodeKind.VariableDeclaration:
                        EvaluateVariableDeclaration(statement as BoundVariableDeclaration);
                        index++;
                        break;

                    case BoundNodeKind.ExpressionStatement:
                        EvaluateExpressionStatement(statement as BoundExpressionStatement);
                        index++;
                        break;

                    case BoundNodeKind.GotoStatement:
                        var goToStatement = statement as BoundGotoStatement;
                        index = labelIndexes[goToStatement.Label];
                        break;

                    case BoundNodeKind.ConditionalGotoStatement:
                        var conditionalGoToStatement = statement as BoundConditionalGotoStatement;
                        var condition = (bool)EvaluateExpression(conditionalGoToStatement.Condition);
                        if (condition == conditionalGoToStatement.JumpIfTrue)
                            index = labelIndexes[conditionalGoToStatement.Label];
                        else
                            index++;
                        break;

                    case BoundNodeKind.LabelStatement:
                        index++;
                        break;

                    default:
                        throw new UnexpectedNodeException($"Unexpected node {statement.Kind}");
                }

            }

            return _lastValue;
        }

        private void EvaluateVariableDeclaration(BoundVariableDeclaration variableDeclaration)
        {
            var value = EvaluateExpression(variableDeclaration.Initializer);
            _variables[variableDeclaration.Variable] = value;
            _lastValue = value;
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement expressionStatement)
        {
            _lastValue = EvaluateExpression(expressionStatement.Expression);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.BracketExpression:
                    return EvaluateBracketExpression(node as BoundBracketExpression);

                case BoundNodeKind.NumberExpression:
                    return EvaluateLiteralExpression(node as BoundLiteralExpression);

                case BoundNodeKind.VariableExpression:
                    return EvaluateVariableExpression(node as BoundVariableExpression);

                case BoundNodeKind.AssignmentExpression:
                    return EvaluateAssignmentExpression(node as BoundAssignmentExpression);

                case BoundNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression(node as BoundUnaryExpression);

                case BoundNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression(node as BoundBinaryExpression);

                case BoundNodeKind.CallExpression:
                    return EvaluateCallExpression(node as BoundCallExpression);

                case BoundNodeKind.ConvertExpression:
                    return EvaluateConvertExpression(node as BoundConvertExpression);

                default:
                    throw new UnexpectedNodeException($"Unexpected Node {node.Kind}");
            }
        }

        private object EvaluateConvertExpression(BoundConvertExpression convertExpression)
        {
            var value = EvaluateExpression(convertExpression.Expression);

            if (convertExpression.Type == SMType.Bool)
                return Convert.ToBoolean(value);

            if (convertExpression.Type == SMType.Int)
                return Convert.ToInt32(value);

            if (convertExpression.Type == SMType.String)
                return Convert.ToString(value);

            throw new UnexpectedTypeException($"Unexpected type {convertExpression.Type}");
        }

        private object EvaluateCallExpression(BoundCallExpression node)
        {
            if (node.Function == BasicFunctions.Input)
            {
                var input = new InputWindow();
                input.ShowDialog();

                var text = input.Input;
                return text;
            }

            if (node.Function == BasicFunctions.Print)
            {
                var message = (string)EvaluateExpression(node.Arguments[0]);
                Output.Add(message);
                return null;
            }

            if (node.Function == BasicFunctions.Random)
            {
                var max = (int)EvaluateExpression(node.Arguments[0]);
                if (_random == null)
                    _random = new Random();

                return _random.Next(max);
            }


            throw new UnexpectedFunctionException($"Unexpected function {node.Function}");
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression binaryExpression)
        {
            var left = EvaluateExpression(binaryExpression.Left);
            var right = EvaluateExpression(binaryExpression.Right);

            switch (binaryExpression.OperatorKind.Kind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return (int)left + (int)right;

                case BoundBinaryOperatorKind.Subtraction:
                    return (int)left - (int)right;

                case BoundBinaryOperatorKind.Multiplication:
                    return (int)left * (int)right;

                case BoundBinaryOperatorKind.Division:
                    return (int)left / (int)right;

                case BoundBinaryOperatorKind.LogicalAnd:
                    return (bool)left && (bool)right;

                case BoundBinaryOperatorKind.LogicalOr:
                    return (bool)left || (bool)right;

                case BoundBinaryOperatorKind.LogicalEquals:
                    return Equals(left, right);

                case BoundBinaryOperatorKind.LogicalNotEquals:
                    return !Equals(left, right);

                case BoundBinaryOperatorKind.LogicalLess:
                    return (int)left < (int)right;

                case BoundBinaryOperatorKind.LogicalLessEquals:
                    return (int)left <= (int)right;

                case BoundBinaryOperatorKind.LogicalGreater:
                    return (int)left > (int)right;

                case BoundBinaryOperatorKind.LogicalGreaterEquals:
                    return (int)left > (int)right;

                default:
                    throw new UnexpectedOperatorException($"Unexpected Binary Operator {binaryExpression.OperatorKind}");
            }
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression unaryExpression)
        {
            var operand = EvaluateExpression(unaryExpression.Operand);

            switch (unaryExpression.OperatorKind.OperatorKind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return (int)operand;

                case BoundUnaryOperatorKind.Negation:
                    return -(int)operand;

                case BoundUnaryOperatorKind.LogicalNegation:
                    return !(bool)operand;

                default:
                    throw new UnexpectedOperatorException($"Unexpected Unary Operator {unaryExpression.OperatorKind}");
            }
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression assignmentExpression)
        {
            var value = EvaluateExpression(assignmentExpression.BoundExpression);
            _variables[assignmentExpression.Variable] = value;
            return value;
        }

        private object EvaluateVariableExpression(BoundVariableExpression variableExpression)
        {
            return _variables[variableExpression.Variable];
        }

        private object EvaluateLiteralExpression(BoundLiteralExpression numberExpression)
        {
            return numberExpression.Value;
        }

        private object EvaluateBracketExpression(BoundBracketExpression boundBracketExpression)
        {
            return EvaluateExpression(boundBracketExpression.Expression);
        }
    }
}
