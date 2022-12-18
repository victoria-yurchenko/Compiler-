using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;

namespace Compiler.CodeAnalysis.Binding.Models.BoundOperators
{
    public sealed class BoundUnaryOperator : BoundExpression
    {
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, SMType operandType)
            : this(syntaxKind, kind, operandType, operandType)
        {
        }

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, SMType operandType, SMType resultType)
        {
            SyntaxKind = syntaxKind;
            OperatorKind = kind;
            Type = operandType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind OperatorKind { get; }
        public override SMType Type { get; }
        public SMType ResultType { get; }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryOperator;

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.LogicalNotToken, BoundUnaryOperatorKind.LogicalNegation, SMType.Bool),
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, SMType.Int),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, SMType.Int)
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, SMType operandType)
        {
            foreach (var operat in _operators)
            {
                if (operat.SyntaxKind == syntaxKind && operat.Type == operandType)
                    return operat;
            }
            return null;
        }
    }
}
