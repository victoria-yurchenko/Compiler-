using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Syntax.Expressions.Models
{
    public sealed class CallExpression : ExpressionSyntax
    {
        public CallExpression(Token identifier, Token openBracket, SeparatedSyntaxList<ExpressionSyntax> arguments, Token closeBracket)
        {
            Identifier = identifier;
            OpenBracket = openBracket;
            Arguments = arguments;
            CloseBracket = closeBracket;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.CallExpression;
        public Token Identifier { get; }
        public Token OpenBracket { get; }
        public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
        public Token CloseBracket { get; }
    }
}