using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(Variable variable)
        {
            Variable = variable;
        }

        public override SMType Type => Variable.Type;
        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
        public Variable Variable { get; }
    }
}
