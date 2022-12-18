using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Syntax.Expressions.Models
{
    public sealed class FunctionParameterTypeSyntax : Node
    {
        public FunctionParameterTypeSyntax( Token colonToken, Token identifier)
        {
            ColonToken = colonToken;
            Identifier = identifier;
        }
        public override SyntaxKind SyntaxKind => SyntaxKind.TypeSyntax;
        public Token Identifier { get; }
        public Token ColonToken { get; }
    }
}
