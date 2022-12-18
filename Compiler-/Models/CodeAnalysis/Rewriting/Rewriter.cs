using Compiler.CodeAnalysis.Binding;
using Compiler.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler.CodeAnalysis.Binding.Models.BoundOperators;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler_.Models.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Lowering
{
    public sealed class Rewriter : BoundTreeRewriter
    {
        private int _labelCount;

        private Rewriter()
        {
        }

        private BoundLabel GenerateLabel()
        {
            var name = $"Label{_labelCount++}";
            return new BoundLabel(name);
        }

        public static BoundBlockStatement Rewrite(BoundStatement statement)
        {
            var lowerer = new Rewriter();
            var result = lowerer.RewriteStatement(statement);
            return GetBoundBlockStatement(result);
        }

        private static BoundBlockStatement GetBoundBlockStatement(BoundStatement statement)
        {
            var builder = ImmutableArray.CreateBuilder<BoundStatement>();
            var stack = new Stack<BoundStatement>();
            stack.Push(statement);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (current is BoundBlockStatement block)
                {
                    foreach (var s in block.Statements.ToImmutableArray().Reverse())
                        stack.Push(s);
                }
                else
                {
                    builder.Add(current);
                }
            }

            return new BoundBlockStatement(builder.ToList());
        }

        protected override BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            if(node.ElseStatement == null)
            {
                var endLabel = GenerateLabel();
                var gotoFalse = new BoundConditionalGotoStatement(endLabel, node.Condition, true);
                var endLabelStatement = new BoundLabelStatement(endLabel);
                var result = new BoundBlockStatement(ImmutableArray.Create(gotoFalse, node.BoundStatement, endLabelStatement).ToList());
                return RewriteStatement(result);
            }
            else
            {
                var elseLabel = GenerateLabel();
                var endLabel = GenerateLabel();

                var gotoFalse = new BoundConditionalGotoStatement(elseLabel, node.Condition, false);
                var gotoEndStatement = new BoundGotoStatement(endLabel);
                var elseLabelStatement = new BoundLabelStatement(elseLabel);
                var endLabelStatement = new BoundLabelStatement(endLabel);
                var result = new BoundBlockStatement(ImmutableArray.Create(
                    gotoFalse,
                    node.BoundStatement,
                    gotoEndStatement,
                    elseLabelStatement,
                    node.ElseStatement,
                    endLabelStatement
                ).ToList());
                return RewriteStatement(result);
            }
        }

        protected override BoundStatement RewriteForStatement(BoundForStatement node)
        {
            var variableDeclaration = new BoundVariableDeclaration(node.Variable, node.LowerBound);
            var variableExpression = new BoundVariableExpression(node.Variable);
            var upperBoundSymbol = new LocalVariable("upperBound", true, SMType.Int);
            var upperBoundDeclaration = new BoundVariableDeclaration(upperBoundSymbol, node.UppperBound);
            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(SyntaxKind.LessEqualsToken, SMType.Int, SMType.Int),
                new BoundVariableExpression(upperBoundSymbol)
            );
            var increment = new BoundExpressionStatement(
                new BoundAssignmentExpression(
                    node.Variable,
                    new BoundBinaryExpression(
                        variableExpression,
                        BoundBinaryOperator.Bind(SyntaxKind.PlusToken, SMType.Int, SMType.Int),
                        new BoundLiteralExpression(1)
                    )
                )
            );
            var whileBody = new BoundBlockStatement(ImmutableArray.Create(node.Body, increment).ToList());
            var whileStatement = new BoundWhileStatement(condition, whileBody);
            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(variableDeclaration, upperBoundDeclaration, whileStatement).ToList());

            return RewriteStatement(result);
        }

        protected override BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            var continueLabel = GenerateLabel();
            var checkLabel = GenerateLabel();

            var gotoCheck = new BoundGotoStatement(checkLabel);
            var continueLabelStatement = new BoundLabelStatement(continueLabel);
            var checkLabelStatement = new BoundLabelStatement(checkLabel);
            var gotoTrue = new BoundConditionalGotoStatement(continueLabel, node.Condition);

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                gotoCheck,
                continueLabelStatement,
                node.Body,
                checkLabelStatement,
                gotoTrue
            ).ToList());

            return RewriteStatement(result);
        }
    }
}
