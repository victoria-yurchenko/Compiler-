using Compiler.CodeAnalysis.Binding.Enums;
using System.Reflection.Emit;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundIfStatement : BoundStatement
    {
        public BoundIfStatement(BoundExpression condition, BoundStatement boundStatement, BoundStatement elseStatement)
        {
            Condition = condition;
            BoundStatement = boundStatement;
            ElseStatement = elseStatement;
        }

        public BoundExpression Condition { get; }
        public BoundStatement BoundStatement { get; }
        public BoundStatement ElseStatement { get; }
        public override BoundNodeKind Kind => BoundNodeKind.IfStatement;
    }

}
