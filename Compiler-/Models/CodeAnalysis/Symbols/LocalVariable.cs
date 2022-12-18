using Compiler_.Models.CodeAnalysis.Symbols.Enums;

namespace Compiler_.Models.CodeAnalysis.Symbols
{
    public class LocalVariable : Variable
    {
        public LocalVariable(string name, bool isReadOnly, SMType type)
            : base(name, isReadOnly, type)
        {
        }

        public override SymbolKind Kind => SymbolKind.LocalVariable;
    }
}