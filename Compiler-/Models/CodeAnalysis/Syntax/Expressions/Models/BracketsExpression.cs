using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Syntax.Expressions.Models
{
    public sealed class BracketsExpression : ExpressionSyntax
    {
        public BracketsExpression(Token openBracketToken, ExpressionSyntax expression, Token closeBracketToken)
        {
            OpenBracketToken = openBracketToken;
            Expression = expression;
            CloseBracketToken = closeBracketToken;
        }

        public Token OpenBracketToken { get; }
        public ExpressionSyntax Expression { get; }
        public Token CloseBracketToken { get; }
        public override SyntaxKind SyntaxKind => SyntaxKind.BracketsExpression;
    }
}
