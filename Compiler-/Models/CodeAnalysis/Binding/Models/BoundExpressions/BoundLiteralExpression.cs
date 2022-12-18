using System;
using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object value)
        {
            Value = value;

            if (value is bool)
                Type = SMType.Bool;
            else if (value is int)
                Type = SMType.Int;
            else if (value is string)
                Type = SMType.String;
            else
                throw new Exception($"Unknown type {value.GetType()}");
        }

        public object Value { get; }
        public override SMType Type {get;}
        public override BoundNodeKind Kind => BoundNodeKind.NumberExpression;
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
