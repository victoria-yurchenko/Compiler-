using Compiler_.Models.CodeAnalysis.Diagnostics;
using Compiler_.Models.CodeAnalysis.Symbols.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Symbols
{
    public sealed class SMType : Symbol
    {
        private string _type;

        public static readonly SMType Error = new SMType("unknown");
        public static readonly SMType Bool = new SMType("bool");
        public static readonly SMType Int = new SMType("int");
        public static readonly SMType String = new SMType("string");
        public static readonly SMType Void = new SMType("void");

        public SMType(string name)
            : base(name)
        {
            _type = name;
        }

        public override SymbolKind Kind => SymbolKind.Type;

        public override string ToString()
        {
            return _type;
        }
    }
}
