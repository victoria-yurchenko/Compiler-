using Compiler_.Models.CodeAnalysis.Symbols;
using System;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public abstract class BoundExpression : BoundNode
    {
        public abstract SMType Type { get; }
    }
}
