using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Statements
{
    public sealed class If : Statement
    {
        public If(Token ifKeyword, ExpressionSyntax condition,
            Statement thenSyntax, Else elseSyntax)
        {
            IfKeyword = ifKeyword;
            Condition = condition;
            ThenSyntax = thenSyntax;
            ElseSyntax = elseSyntax;
        }

        public Token IfKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public Statement ThenSyntax { get; }
        public Else ElseSyntax { get; }
        public override SyntaxKind SyntaxKind => SyntaxKind.IfStatement;
    }
}
