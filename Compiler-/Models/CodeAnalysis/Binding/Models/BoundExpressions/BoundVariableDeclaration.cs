using Compiler.CodeAnalysis.Binding.Enums;
using Compiler_.Models.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding.Models.BoundExpressions
{
    public sealed class BoundVariableDeclaration : BoundStatement
    {
        public BoundVariableDeclaration(Variable variable, BoundExpression initializer)
        {
            Variable = variable;
            Initializer = initializer;
        }

        public Variable Variable { get; }
        public BoundExpression Initializer { get; }
        public override BoundNodeKind Kind => BoundNodeKind.VariableDeclaration;
    }
}
