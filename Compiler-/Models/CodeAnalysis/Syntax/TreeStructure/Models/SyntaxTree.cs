using Compiler.CodeAnalysis.Diagnostics;
using Compiler.CodeAnalysis.Compilation;
using Compiler.CodeAnalysis.Syntax.Expressions.Models;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.CodeAnalysis.Text;

namespace Compiler.CodeAnalysis.Syntax.TreeStructure.Models
{
    public sealed class SyntaxTree
    {
        private SyntaxTree(SourceText text)
        {
            var parser = new Parser(text);
            var root = parser.ParseCompilationUnit();
            var diagnostics = parser.Diagnostic.ToList();

            Diagnostics = new DiagnosticList(diagnostics);
            Root = root;
            Text = text;
        }

        public SourceText Text { get; }
        public DiagnosticList Diagnostics { get; }
        public CompilationUnit Root { get; }

        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText sourceText)
        {
            return new SyntaxTree(sourceText);
        }
    }
}
