using Compiler_.Models.CodeAnalysis.Symbols.Enums;

namespace Compiler_.Models.CodeAnalysis.Symbols
{
    public sealed class GlobalVariable : Variable
    {
        public GlobalVariable(string name, bool isReadOnly, SMType type)
            : base(name, isReadOnly, type)
        {
        }

        public override SymbolKind Kind => SymbolKind.GlobalVariable;
    }
}