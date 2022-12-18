using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundConvertExpression : BoundExpression
    {
        public BoundConvertExpression(SMType type, BoundExpression expression)
        {
            Type = type;
            Expression = expression;
        }

        public override SMType Type { get; }
        public BoundExpression Expression { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ConvertExpression;
    }
}
