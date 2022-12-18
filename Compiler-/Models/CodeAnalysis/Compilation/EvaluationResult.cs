using Compiler.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.CodeAnalysis.Compilation
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<Diagnostic> diagnostic, object value)
        {
            Diagnostics = diagnostic.ToList();
            Value = value;
        }

        public List<Diagnostic> Diagnostics { get; }
        public object Value { get; }
    }
}
