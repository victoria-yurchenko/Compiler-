using Compiler.CodeAnalysis.Statements;
using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler_.Models.CodeAnalysis.Syntax.Expressions.Models;
using System.Collections.Generic;
using static Compiler.CodeAnalysis.Compilation.CompilationUnit;

namespace Compiler.CodeAnalysis.Compilation
{
    public sealed class FunctionDeclaration : ProgramMember
    {
        public FunctionDeclaration(Token functionKeyword, Token identifier, Token openBracket, SeparatedSyntaxList<ParameterSyntax> parameters, FunctionParameterTypeSyntax functionType, Token closeBracket, BlockStatement body)
        {
            FunctionKeyword = functionKeyword;
            Identifier = identifier;
            OpenBracket = openBracket;
            Parameters = parameters;
            FunctionType = functionType;
            CloseBracket = closeBracket;
            Body = body;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.FunctionDeclaration;
        public Token FunctionKeyword { get; }
        public Token Identifier { get; }
        public Token OpenBracket { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public FunctionParameterTypeSyntax FunctionType { get; }
        public Token CloseBracket { get; }
        public BlockStatement Body { get; }
    }
}