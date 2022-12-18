using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;

namespace Compiler.CodeAnalysis.Binding.Models.BoundOperators
{
    public sealed class BoundBinaryOperator
    {
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, SMType type)
            : this(syntaxKind, kind, type, type, type)
        {
        }

        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, SMType operandType, SMType resultType)
           : this(syntaxKind, kind, operandType, operandType, resultType)
        {
        }

        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, SMType operandTypeLeft, SMType operandTypeRight, SMType resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandTypeLeft = operandTypeLeft;
            OperandTypeRight = operandTypeRight;
            ResultType = resultType;
        }

        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, SMType.Int),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction,SMType.Int),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, SMType.Int),
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, SMType.Int),

            new BoundBinaryOperator(SyntaxKind.LogicalEqualsToken, BoundBinaryOperatorKind.LogicalEquals, SMType.Int,SMType.Bool),
            new BoundBinaryOperator(SyntaxKind.LogicalEqualsToken, BoundBinaryOperatorKind.LogicalEquals, SMType.String,SMType.Bool),
            new BoundBinaryOperator(SyntaxKind.LogicalNotEqualsToken, BoundBinaryOperatorKind.LogicalNotEquals, SMType.Int, SMType.Bool),
            new BoundBinaryOperator(SyntaxKind.LogicalEqualsToken, BoundBinaryOperatorKind.LogicalEquals, SMType.Bool),
            new BoundBinaryOperator(SyntaxKind.LogicalNotEqualsToken, BoundBinaryOperatorKind.LogicalNotEquals, SMType.Bool),

            new BoundBinaryOperator(SyntaxKind.LessToken, BoundBinaryOperatorKind.LogicalLess, SMType.Int, SMType.Bool),
            new BoundBinaryOperator(SyntaxKind.LessEqualsToken, BoundBinaryOperatorKind.LogicalLessEquals, SMType.Int, SMType.Bool),
            new BoundBinaryOperator(SyntaxKind.GreaterToken, BoundBinaryOperatorKind.LogicalGreater, SMType.Int, SMType.Bool),
            new BoundBinaryOperator(SyntaxKind.GreaterEqualsToken, BoundBinaryOperatorKind.LogicalGreaterEquals, SMType.Int,SMType.Bool),

            new BoundBinaryOperator(SyntaxKind.LogicalAndToken, BoundBinaryOperatorKind.LogicalAnd, SMType.Bool),
            new BoundBinaryOperator(SyntaxKind.LogicalOrToken, BoundBinaryOperatorKind.LogicalOr, SMType.Bool),
        };

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public SMType OperandTypeLeft { get; }
        public SMType OperandTypeRight { get; }
        public SMType ResultType { get; }

        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, SMType leftType, SMType rightType)
        {
            foreach (var operat in _operators)
            {
                if (operat.SyntaxKind == syntaxKind
                    && operat.OperandTypeLeft == leftType && operat.OperandTypeRight == rightType)
                    return operat;
            }
            return null;
        }
    }


}
