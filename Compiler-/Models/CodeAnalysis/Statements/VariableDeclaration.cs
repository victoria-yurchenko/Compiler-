using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler_.Models.CodeAnalysis.Syntax.Expressions.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Statements
{
    public sealed class VariableDeclaration : Statement
    {
        public VariableDeclaration(Token keyword, Token identifier, FunctionParameterTypeSyntax typeSyntax, Token equalsToken, ExpressionSyntax initializer)
        {
            Keyword = keyword;
            Identifier = identifier;
            TypeSyntax = typeSyntax;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.VariableDeclaration;

        public Token Keyword { get; }
        public Token Identifier { get; }
        public FunctionParameterTypeSyntax TypeSyntax { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }
    }
}
