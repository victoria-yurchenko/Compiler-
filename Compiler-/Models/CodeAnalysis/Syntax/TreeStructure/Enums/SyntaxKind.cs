using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Syntax.TreeStructure.Enums
{
    public enum SyntaxKind
    {
        //Tokens
        LiteralToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenBracketToken,
        CloseBracketToken,
        BadToken,
        EndOfFileToken,
        IdentifierToken,
        LogicalNotToken,
        LogicalAndToken,
        LogicalOrToken,

        //Expressions
        NumberExpression,
        BinaryExpression,
        BracketsExpression,
        UnaryExpression,

        //Keywords
        TrueKeyword,
        FalseKeyword,
        LogicalNotEqualsToken,
        LogicalEqualsToken,
        VariableNameExpression,
        VariableAssignmentExpression,
        EqualsToken,
        CompilationUnit,
        BlockStatement,
        ExpressionStatement,
        OpenBraceToken,
        CloseBraceToken,
        LessToken,
        LessEqualsToken,
        GreaterToken,
        GreaterEqualsToken,
        IfStatement,
        ElseSyntax,
        VariableDeclaration,
        ConstantKeyword,
        VariableKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,
        WhileStatement,
        ForKeyword,
        ForStatement,
        ToKeyword,
        StringToken,
        CallExpression,
        CommaToken,
        GlobalStatement,
        FunctionDeclaration,
        TypeSyntax,
        ColonToken,
        Parameter,
        FunctionKeyword
    }
}
