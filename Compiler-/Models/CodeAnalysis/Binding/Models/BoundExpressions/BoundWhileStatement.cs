using Compiler.CodeAnalysis.Binding.Enums;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{

    public sealed class BoundWhileStatement : BoundStatement
    {
        public BoundWhileStatement(BoundExpression condition, BoundStatement body)
        {
            Condition = condition;
            Body = body;
        }

        public BoundExpression Condition { get; }
        public BoundStatement Body { get; }
        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;
    }
}
