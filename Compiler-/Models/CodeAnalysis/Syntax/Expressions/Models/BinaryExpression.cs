using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Syntax.Expressions.Models
{
    public sealed class BinaryExpression : ExpressionSyntax
    {
        public BinaryExpression(ExpressionSyntax left, Token operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.BinaryExpression;
        public ExpressionSyntax Left { get; }
        public Token OperatorToken { get; }
        public ExpressionSyntax Right { get; }
    }
}
