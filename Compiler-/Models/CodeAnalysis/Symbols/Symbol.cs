using System;
using System.Collections.Generic;
using System.Text;
using Compiler_.Models.CodeAnalysis.Symbols.Enums;

namespace Compiler_.Models.CodeAnalysis.Diagnostics
{
    public abstract class Symbol
    {
        public Symbol(string name)
        {
            Name = name;
        }

        public abstract SymbolKind Kind { get; }
        public string Name { get; }
    }
}
