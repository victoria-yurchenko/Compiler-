using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Statements
{
    public sealed class Else : Node
    {
        public Else(Token elseKeyword, Statement elseStatement)
        {
            ElseKeyword = elseKeyword;
            ElseStatement = elseStatement;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.ElseSyntax;

        public Token ElseKeyword { get; }
        public Statement ElseStatement { get; }
    }
}
