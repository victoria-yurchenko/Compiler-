using Compiler.CodeAnalysis.Binding.Models.Scope;
using Compiler.CodeAnalysis.Diagnostics;
using Compiler_.Models.CodeAnalysis.Symbols;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundProgram
    {
        public BoundProgram( DiagnosticList diagnostics, Dictionary<Function, BoundBlockStatement> functionBodies, BoundBlockStatement statement)//BoundGlobalScope globalScope,
        {
            Diagnostics = diagnostics;
            FunctionBodies = functionBodies;
            Statement = statement;
        }

        public DiagnosticList Diagnostics { get; }
        public Dictionary<Function, BoundBlockStatement> FunctionBodies { get; }
        public BoundBlockStatement Statement { get; }
    }
}
