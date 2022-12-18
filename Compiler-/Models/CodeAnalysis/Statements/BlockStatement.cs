using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Statements
{
    public sealed class BlockStatement : Statement
    {
        public BlockStatement(Token openBraceToken, List<Statement> statements, Token closeBraceToken)
        {
            OpenBraceToken = openBraceToken;
            Statements = statements;
            CloseBraceToken = closeBraceToken;
        }

        public Token OpenBraceToken { get; }
        public List<Statement> Statements { get; }
        public Token CloseBraceToken { get; }
        public override SyntaxKind SyntaxKind => SyntaxKind.BlockStatement;
    }
}
