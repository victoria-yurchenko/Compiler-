using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler_.Models.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundGotoStatement : BoundStatement
    {
        public BoundGotoStatement(BoundLabel label)
        {
            Label = label;
        }

        public override BoundNodeKind Kind => BoundNodeKind.GotoStatement;
        public BoundLabel Label { get; }
    }

}
