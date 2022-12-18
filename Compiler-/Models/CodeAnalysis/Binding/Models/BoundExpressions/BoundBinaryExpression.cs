using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Binding.Models.BoundOperators;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator operatorKind, BoundExpression right)
        {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }

        public override SMType Type => OperatorKind.ResultType;
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        public BoundExpression Left { get; }
        public BoundBinaryOperator OperatorKind { get; }
        public BoundExpression Right { get; }
    }
}
