using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundCallExpression : BoundExpression
    {
        public BoundCallExpression(Function function, List<BoundExpression> arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        public Function Function { get; }
        public List<BoundExpression> Arguments { get; }
        public override SMType Type => Function.ReturnType;
        public override BoundNodeKind Kind => BoundNodeKind.CallExpression;
    }
}
