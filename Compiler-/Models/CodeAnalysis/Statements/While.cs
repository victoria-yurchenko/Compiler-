using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Statements
{
    public sealed class While : Statement
    {
        public While(Token whileKeyword, ExpressionSyntax condition, Statement body)
        {
            WhileKeyword = whileKeyword;
            Condition = condition;
            Body = body;
        }

        public Token WhileKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public Statement Body { get; }
        public override SyntaxKind SyntaxKind => SyntaxKind.WhileStatement;
    }
}
