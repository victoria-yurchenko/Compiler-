using Compiler.CodeAnalysis.Compilation;
using Compiler_.Models.CodeAnalysis.Diagnostics;
using Compiler_.Models.CodeAnalysis.Symbols.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static Compiler.CodeAnalysis.Compilation.CompilationUnit;

namespace Compiler_.Models.CodeAnalysis.Symbols
{
    public sealed class Function : Symbol
    {
        public Function(string name, List<Parameter> parameters, SMType returnType, FunctionDeclaration declaration = null)
            : base(name)
        {
            Parameters = parameters;
            ReturnType = returnType;
            Declaration = declaration;
        }

        public override SymbolKind Kind => SymbolKind.Function;
        public List<Parameter> Parameters { get; }
        public SMType ReturnType { get; }
        public FunctionDeclaration Declaration { get; }
    }
}
