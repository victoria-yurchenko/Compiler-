using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Syntax.Expressions.Models
{
    public sealed class VariableNameExpression : ExpressionSyntax
    {
        public VariableNameExpression(Token identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public Token IdentifierToken { get; }
        public override SyntaxKind SyntaxKind => SyntaxKind.VariableNameExpression;
    }
}
