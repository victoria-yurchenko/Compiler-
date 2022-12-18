using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public class BoundBracketExpression : BoundExpression
    {
        public BoundBracketExpression(BoundExpression expression)
        {
            Expression = expression;
        }

        public override SMType Type => Expression.Type;
        public override BoundNodeKind Kind => BoundNodeKind.BracketExpression;
        public BoundExpression Expression { get; }
    }
}
