using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundForStatement : BoundStatement
    {
        public BoundForStatement(Variable variable, BoundExpression lowerBound, BoundExpression uppperBound, BoundStatement body)
        {
            Variable = variable;
            LowerBound = lowerBound;
            UppperBound = uppperBound;
            Body = body;
        }

        public override BoundNodeKind Kind => BoundNodeKind.ForStatement;
        public Variable Variable { get; }
        public BoundExpression LowerBound { get; }
        public BoundExpression UppperBound { get; }
        public BoundStatement Body { get; }
    }
}
