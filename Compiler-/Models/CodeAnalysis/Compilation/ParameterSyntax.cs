using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler_.Models.CodeAnalysis.Syntax.Expressions.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Compilation
{
    public sealed partial class CompilationUnit
    {
        public sealed class ParameterSyntax : Node
        {
            public ParameterSyntax(Token identifier, FunctionParameterTypeSyntax type)
            {
                Identifier = identifier;
                Type = type;
            }

            public override SyntaxKind SyntaxKind => SyntaxKind.Parameter;
            public Token Identifier { get; }
            public FunctionParameterTypeSyntax Type { get; }
        }
    }
}