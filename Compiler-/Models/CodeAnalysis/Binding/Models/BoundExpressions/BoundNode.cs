using Compiler.CodeAnalysis.Binding.Enums;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public abstract class BoundNode
    {
        public abstract BoundNodeKind Kind { get; }
    }
}
