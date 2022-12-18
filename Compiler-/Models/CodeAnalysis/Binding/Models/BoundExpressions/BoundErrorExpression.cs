using Compiler.CodeAnalysis.Binding.Enums;
using Compiler.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Binding.Models.BoundExpressions
{
    internal class BoundErrorExpression : BoundExpression
    {
        public override SMType Type => SMType.Error;
        public override BoundNodeKind Kind => BoundNodeKind.ErrorExpression;
    }
}
