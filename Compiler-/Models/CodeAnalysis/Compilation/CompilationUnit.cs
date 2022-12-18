using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler_.Models.CodeAnalysis.Symbols;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Compilation
{
    public sealed partial class CompilationUnit : Node
    {
        public CompilationUnit(List<ProgramMember> members, Token endOfFileToken)
        {
            Members = members;
            EndOfFileToken = endOfFileToken;
        }

        public List<ProgramMember> Members { get; }
        public Token EndOfFileToken { get; }
        public override SyntaxKind SyntaxKind => SyntaxKind.CompilationUnit;
    }
}