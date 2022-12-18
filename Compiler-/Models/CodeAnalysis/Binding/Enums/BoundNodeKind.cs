namespace Compiler.CodeAnalysis.Binding.Enums
{
    public enum BoundNodeKind
    {
        UnaryExpression,
        NumberExpression,
        BinaryExpression,
        BracketExpression,
        VariableExpression,
        AssignmentExpression,
        BlockStatement,
        ExpressionStatement,
        VariableDeclaration,
        IfStatement,
        WhileStatement,
        ForStatement,
        ErrorExpression,
        CallExpression,
        ConvertExpression,
        GotoStatement,
        LabelStatement,
        ConditionalGotoStatement,
        UnaryOperator
    }
}
