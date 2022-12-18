using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Syntax.TreeStructure.Models
{
    public static class SyntaxAxioms
    {
        public static int GetUnaryOperatorPriority(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.LogicalNotToken:
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 6;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPriority(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;

                case SyntaxKind.LogicalEqualsToken:
                case SyntaxKind.LogicalNotEqualsToken:
                case SyntaxKind.GreaterEqualsToken:
                case SyntaxKind.GreaterToken:
                case SyntaxKind.LessEqualsToken:
                case SyntaxKind.LessToken:
                    return 3;

                case SyntaxKind.LogicalAndToken:
                    return 2;

                case SyntaxKind.LogicalOrToken:
                    return 1;

                default:
                    return 0;
            }
        }

        internal static SyntaxKind GetKeywordSyntaxKind(string text)
        {
            switch (text)
            {
                case "true":
                    return SyntaxKind.TrueKeyword;

                case "false":
                    return SyntaxKind.FalseKeyword;

                case "constant":
                    return SyntaxKind.ConstantKeyword;

                case "variable":
                    return SyntaxKind.VariableKeyword;

                case "if":
                    return SyntaxKind.IfKeyword;

                case "else":
                    return SyntaxKind.ElseKeyword;

                case "while":
                    return SyntaxKind.WhileKeyword;

                case "for":
                    return SyntaxKind.ForKeyword;

                case "to":
                    return SyntaxKind.ToKeyword;

                case "function":
                    return SyntaxKind.FunctionKeyword;

                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
        
        public static string GetTextFromSyntaxKind(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.WhitespaceToken:
                    return " ";

                case SyntaxKind.PlusToken:
                    return "+";

                case SyntaxKind.LessToken:
                    return "<";

                case SyntaxKind.LessEqualsToken:
                    return "<=";

                case SyntaxKind.GreaterToken:
                    return ">";

                case SyntaxKind.GreaterEqualsToken:
                    return ">=";

                case SyntaxKind.MinusToken:
                    return "-";

                case SyntaxKind.StarToken:
                    return "*";

                case SyntaxKind.SlashToken:
                    return "/";

                case SyntaxKind.OpenBracketToken:
                    return "(";

                case SyntaxKind.CloseBracketToken:
                    return ")";

                case SyntaxKind.OpenBraceToken:
                    return "{";

                case SyntaxKind.CloseBraceToken:
                    return "}";

                case SyntaxKind.CommaToken:
                    return ",";

                case SyntaxKind.ColonToken:
                    return ":";

                case SyntaxKind.EndOfFileToken:
                    return "\0";

                case SyntaxKind.LogicalNotToken:
                    return "!";

                case SyntaxKind.LogicalAndToken:
                    return "&&";

                case SyntaxKind.LogicalOrToken:
                    return "||";

                case SyntaxKind.TrueKeyword:
                    return "true";

                case SyntaxKind.FalseKeyword:
                    return "false";

                case SyntaxKind.ConstantKeyword:
                    return "constant";

                case SyntaxKind.VariableKeyword:
                    return "variable";

                case SyntaxKind.IfKeyword:
                    return "if";

                case SyntaxKind.ElseKeyword:
                    return "else";

                case SyntaxKind.WhileKeyword:
                    return "while";

                case SyntaxKind.ForKeyword:
                    return "for";

                case SyntaxKind.ToKeyword:
                    return "to";

                case SyntaxKind.FunctionKeyword:
                    return "function";

                case SyntaxKind.LogicalNotEqualsToken:
                    return "!=";

                case SyntaxKind.LogicalEqualsToken:
                    return "==";

                case SyntaxKind.EqualsToken:
                    return "=";

                default:
                    return null;
            }
        }
    }
}
