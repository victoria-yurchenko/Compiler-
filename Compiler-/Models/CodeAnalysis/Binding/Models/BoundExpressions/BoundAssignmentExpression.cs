using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(Variable name, BoundExpression boundExpression)
        {
            Variable = name;
            BoundExpression = boundExpression;
        }

        public override SMType Type => BoundExpression.Type;
        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public Variable Variable { get; }
        public BoundExpression BoundExpression { get; }
    }
}
