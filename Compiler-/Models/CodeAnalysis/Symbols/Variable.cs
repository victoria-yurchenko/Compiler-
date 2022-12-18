using Compiler_.Models.CodeAnalysis.Diagnostics;
using Compiler_.Models.CodeAnalysis.Symbols.Enums;
using System;

namespace Compiler_.Models.CodeAnalysis.Symbols
{
    public abstract class Variable : Symbol
    {
        public Variable(string name, bool isReadOnly, SMType type)
            : base(name)
        {
            IsReadOnly = isReadOnly;
            Type = type;
        }
        public bool IsReadOnly { get; }
        public SMType Type { get; }
        public override SymbolKind Kind => SymbolKind.Variable;
    }
}