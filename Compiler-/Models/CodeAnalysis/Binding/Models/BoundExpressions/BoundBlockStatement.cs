using Compiler.CodeAnalysis.Binding.Enums;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundBlockStatement : BoundStatement
    {
        public BoundBlockStatement(List<BoundStatement> statements)
        {
            Statements = statements;
        }

        public List<BoundStatement> Statements { get; }
        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;
    }
}
