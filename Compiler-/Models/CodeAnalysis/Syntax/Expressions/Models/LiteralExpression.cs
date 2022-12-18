using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Syntax.Expressions.Models
{
    public sealed class LiteralExpression : ExpressionSyntax
    {
        public LiteralExpression(Token numberToken, object value)
        {
            NumberToken = numberToken;
            Value = value;
        }

        public LiteralExpression(Token numberToken) : this(numberToken, numberToken.Value)
        {
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.NumberExpression;
        public Token NumberToken { get; }
        public object Value { get; }
    }
}
