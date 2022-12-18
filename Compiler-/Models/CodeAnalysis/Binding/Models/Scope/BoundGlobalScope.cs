using Compiler.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler.CodeAnalysis.Diagnostics;
using Compiler_.Models.CodeAnalysis.Symbols;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Binding.Models.Scope
{
    public sealed class BoundGlobalScope
    {
        public BoundGlobalScope(BoundGlobalScope previous, IReadOnlyList<Diagnostic> diagnostics, IReadOnlyList<Function> functions, IReadOnlyList<Variable> variables, List<BoundStatement> statements)//IReadOnlyList<BoundStatement> statements
        {
            Previous = previous;
            Diagnostics = diagnostics;
            Functions = functions;
            Variables = variables;
            Statements = statements;
        }

        public BoundGlobalScope Previous { get; }
        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public IReadOnlyList<Function> Functions { get; }
        public IReadOnlyList<Variable> Variables { get; }
        public BoundStatement Statement { get; }
        public List<BoundStatement> Statements { get; }
    }
}
