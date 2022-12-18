using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler.CodeAnalysis.Text;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Syntax.Expressions.Models
{
    public sealed class VariableAssignmentExpression : ExpressionSyntax
    {
        public VariableAssignmentExpression(Token identifierToken, Token equalsToken, ExpressionSyntax expression)
        {
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public Token IdentifierToken { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public override SyntaxKind SyntaxKind => SyntaxKind.VariableAssignmentExpression;
    }
}