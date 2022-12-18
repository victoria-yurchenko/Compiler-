using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Binding.Models.BoundExpressions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Documents;
using System.Xml.Linq;

namespace Compiler.CodeAnalysis.Binding
{
    public abstract class BoundTreeRewriter
    {
        public virtual BoundStatement RewriteStatement(BoundStatement node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.BlockStatement:
                    return RewriteBlockStatement(node as BoundBlockStatement);

                case BoundNodeKind.VariableDeclaration:
                    return RewriteVariableDeclarationStatement(node as BoundVariableDeclaration);

                case BoundNodeKind.IfStatement:
                    return RewriteIfStatement(node as BoundIfStatement);

                case BoundNodeKind.WhileStatement:
                    return RewriteWhileStatement(node as BoundWhileStatement);

                case BoundNodeKind.ForStatement:
                    return RewriteForStatement(node as BoundForStatement);

                case BoundNodeKind.ExpressionStatement:
                    return RewriteExpressionStatement(node as BoundExpressionStatement);

                case BoundNodeKind.LabelStatement:
                    return RewriteLabelStatement(node as BoundLabelStatement);

                case BoundNodeKind.GotoStatement:
                    return RewriteGotoStatement(node as BoundGotoStatement);

                case BoundNodeKind.ConditionalGotoStatement:
                    return RewriteConditionalGotoStatement(node as BoundConditionalGotoStatement);

                default:
                    throw new Exception($"Unexected node {node.Kind}");
            }
        }

        protected virtual BoundStatement RewriteConditionalGotoStatement(BoundConditionalGotoStatement node)
        {
            var condition = RewriteExpression(node.Condition);

            if (condition == node.Condition)
                return node;

            return new BoundConditionalGotoStatement(node.Label, condition, node.JumpIfTrue);
        }

        protected virtual BoundStatement RewriteGotoStatement(BoundGotoStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteLabelStatement(BoundLabelStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteExpressionStatement(BoundExpressionStatement node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
                return node;

            return new BoundExpressionStatement(expression);
        }

        protected virtual BoundStatement RewriteForStatement(BoundForStatement node)
        {
            var lowerBound = RewriteExpression(node.LowerBound);
            var upperBound = RewriteExpression(node.UppperBound);
            var body = RewriteStatement(node.Body);

            if (lowerBound == node.LowerBound &&
                upperBound == node.UppperBound &&
                body == node.Body)
            {
                return node;
            }

            return new BoundForStatement(node.Variable, lowerBound, upperBound, body);
        }

        protected virtual BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var body = RewriteStatement(node.Body);

            if (condition == node.Condition && body == node.Body)
                return node;

            return new BoundWhileStatement(condition, body);
        }

        protected virtual BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var boundStatement = RewriteStatement(node.BoundStatement);
            var elseStatement = node == null ? null : RewriteStatement(node.ElseStatement);

            if (condition == node.Condition &&
                boundStatement == node.BoundStatement &&
                elseStatement == node.ElseStatement)
            {
                return node;
            }

            return new BoundIfStatement(condition, boundStatement, elseStatement);
        }

        protected virtual BoundStatement RewriteVariableDeclarationStatement(BoundVariableDeclaration node)
        {
            var initializer = RewriteExpression(node.Initializer);

            if (initializer == node.Initializer)
                return node;

            return new BoundVariableDeclaration(node.Variable, initializer);
        }

        protected virtual BoundStatement RewriteBlockStatement(BoundBlockStatement node)
        {
            List<BoundStatement> statements = null;

            for (int i = 0; i < node.Statements.Count; i++)
            {
                var oldStatement = node.Statements[i];
                var newStatement = RewriteStatement(oldStatement);
                if (newStatement != oldStatement)
                {
                    if (statements == null)
                    {
                        statements = new List<BoundStatement>(node.Statements.Count);

                        for (int j = 0; j < i; j++)
                        {
                            statements.Add(node.Statements[j]);
                        }
                    }
                }

                if (statements != null)
                {
                    statements.Add(newStatement);
                }
            }

            if (statements == null)
                return node;

            return new BoundBlockStatement(statements);
        }

        protected virtual BoundExpression RewriteExpression(BoundExpression node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.BinaryExpression:
                    return RewriteBinaryExpression(node as BoundBinaryExpression);

                case BoundNodeKind.NumberExpression:
                    return RewriteNumberExpression(node as BoundLiteralExpression);

                case BoundNodeKind.VariableExpression:
                    return RewriteVariableExpression(node as BoundVariableExpression);

                case BoundNodeKind.AssignmentExpression:
                    return RewriteAssignmentExpression(node as BoundAssignmentExpression);

                case BoundNodeKind.UnaryExpression:
                    return RewriteUnaryExpression(node as BoundUnaryExpression);

                case BoundNodeKind.CallExpression:
                    return RewriteCallExpression(node as BoundCallExpression);

                case BoundNodeKind.ConvertExpression:
                    return RewriteConvertExpression(node as BoundConvertExpression);

                default:
                    throw new Exception($"Unexected node {node.Kind}");
            }
        }

        private BoundExpression RewriteConvertExpression(BoundConvertExpression node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
                return node;

            return new BoundConvertExpression(node.Type, expression);
        }

        private BoundExpression RewriteCallExpression(BoundCallExpression node)
        {
            List<BoundExpression> arguments = null;

            for (var i = 0; i < node.Arguments.Count; i++)
            {
                var oldArgument = node.Arguments[i];
                var newArgument = RewriteExpression(oldArgument);
                if (newArgument != oldArgument)
                {
                    if (arguments == null)
                    {
                        for (var j = 0; j < i; j++)
                            arguments.Add(node.Arguments[j]);
                    }
                }

                if (arguments != null)
                    arguments.Add(newArgument);
            }

            if (arguments == null)
                return node;

            return new BoundCallExpression(node.Function, arguments);
        }

        protected virtual BoundExpression RewriteUnaryExpression(BoundUnaryExpression node)
        {
            var operand = RewriteExpression(node.Operand);
            if (operand == node.Operand)
                return node;

            return new BoundUnaryExpression(node.OperatorKind, operand);
        }

        protected virtual BoundExpression RewriteAssignmentExpression(BoundAssignmentExpression node)
        {
            var expression = RewriteExpression(node.BoundExpression);
            if (expression == node.BoundExpression)
                return node;

            return new BoundAssignmentExpression(node.Variable, expression);
        }

        protected virtual BoundExpression RewriteVariableExpression(BoundVariableExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteNumberExpression(BoundLiteralExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteBinaryExpression(BoundBinaryExpression node)
        {
            var left = RewriteExpression(node.Left);
            var right = RewriteExpression(node.Right);
            if (left == node.Left && right == node.Right)
                return node;

            return new BoundBinaryExpression(left, node.OperatorKind, right);
        }
    }
}
