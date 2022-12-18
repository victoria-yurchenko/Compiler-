using Compiler.CodeAnalysis.Binding.Enums;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundExpressionStatement : BoundStatement
    {
        public BoundExpressionStatement(BoundExpression expression)
        {
            Expression = expression;
        }

        public BoundExpression Expression { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;
    }
}
