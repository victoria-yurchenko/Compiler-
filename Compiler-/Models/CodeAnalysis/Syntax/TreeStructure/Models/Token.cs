using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.CodeAnalysis.Syntax.TreeStructure.Models
{
    public class Token : Node
    {
        public Token(SyntaxKind kind, int position, string text, object value)
        {
            SyntaxKind = kind;
            Position = position;

            if (text == null)
                Text = string.Empty;
            else
                Text = text;

            Value = value;
        }

        public override SyntaxKind SyntaxKind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
    }
}
