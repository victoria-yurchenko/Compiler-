using Compiler_.Models.CodeAnalysis.Diagnostics;
using Compiler_.Models.CodeAnalysis.Symbols.Enums;

namespace Compiler_.Models.CodeAnalysis.Symbols
{
    public sealed class Parameter : LocalVariable
    {
        public Parameter(string name, SMType type)
            :base(name, true, type)
        {
        }

        public override SymbolKind Kind => SymbolKind.Parameter;
    }
}
