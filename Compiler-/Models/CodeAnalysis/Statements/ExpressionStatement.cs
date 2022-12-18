using Compiler.CodeAnalysis.Statements;
using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler_.Models.CodeAnalysis.Statements
{
    public sealed class ExpressionStatement : Statement
    {
        public ExpressionStatement(ExpressionSyntax expression)
        {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }
        public override SyntaxKind SyntaxKind => SyntaxKind.ExpressionStatement;
    }
}
