using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler_.Models.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundConditionalGotoStatement : BoundStatement
    {
        public BoundConditionalGotoStatement(BoundLabel label, BoundExpression condition, bool jumpIfTrue = true)
        {
            Label = label;
            Condition = condition;
            JumpIfTrue = jumpIfTrue;
        }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatement;
        public BoundLabel Label { get; }
        public BoundExpression Condition { get; }
        public bool JumpIfTrue { get; }
    }

}
