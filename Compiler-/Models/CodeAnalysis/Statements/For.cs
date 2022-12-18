using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Statements
{
    public sealed class For : Statement
    {
        public For(Token forKeyword, Token identifier, Token equalsToken, ExpressionSyntax lowerBound, Token toKeyword, ExpressionSyntax upperBound, Statement body)
        {
            ForKeyword = forKeyword;
            Identifier = identifier;
            EqualsToken = equalsToken;
            LowerBound = lowerBound;
            ToKeyword = toKeyword;
            UpperBound = upperBound;
            Body = body;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.ForStatement;
        public Token ForKeyword { get; }
        public Token Identifier { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax LowerBound { get; }
        public Token ToKeyword { get; }
        public ExpressionSyntax UpperBound { get; }
        public Statement Body { get; }
    }
}
