using System;
using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Binding.Models.BoundOperators;
using Compiler_.Models.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator operatorKind, BoundExpression operand)
        {
            OperatorKind = operatorKind;
            Operand = operand;
        }

        public override SMType Type => OperatorKind.ResultType;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public BoundUnaryOperator OperatorKind { get; }
        public BoundExpression Operand { get; }
    }
}
