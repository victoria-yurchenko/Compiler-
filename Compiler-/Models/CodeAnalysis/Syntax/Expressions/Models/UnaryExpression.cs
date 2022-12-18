using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Syntax.Expressions.Models
{
    public sealed class UnaryExpression : ExpressionSyntax
    {
        public UnaryExpression(Token operatorToken, ExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.UnaryExpression;
        public Token OperatorToken { get; }
        public ExpressionSyntax Operand { get; }
    }
}
