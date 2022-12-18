using Compiler.CodeAnalysis.Statements;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;
using static Compiler.CodeAnalysis.Compilation.CompilationUnit;

namespace Compiler.CodeAnalysis.Compilation
{
    public sealed class GlobalStatement : ProgramMember
    {
        public GlobalStatement(Statement statement)
        {
            Statement = statement;
        }

        public Statement Statement { get; }
        public override SyntaxKind SyntaxKind => SyntaxKind.GlobalStatement;
    }
}