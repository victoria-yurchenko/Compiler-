using Compiler.CodeAnalysis.Compilation;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler.CodeAnalysis.Text;
using Compiler_.Models.CodeAnalysis.Symbols;
using Compler.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_.ViewModels
{
    public class Representation
    {
        public Representation(string input)
        {
            TextInput = input;
            RepresentCompilation();
        }

        public string TextInput { get; set; }
        public string TextOutput { get; private set; }

        private void RepresentCompilation()
        {
            TextOutput = string.Empty;
            var variables = new Dictionary<Variable, object>();

            if (!TextInput.Contains('\0'))
                TextInput += "\0";

            var text = TextInput;
            var syntaxTree = SyntaxTree.Parse(text);

            var compilation = new Compilation(syntaxTree);
            var evaluationResult = compilation.Evaluate(variables);
            var diagnostics = evaluationResult.Diagnostics;

            if (Evaluator.Output != null && Evaluator.Output.Count != 0)
            {
                foreach (var message in Evaluator.Output) 
                    TextOutput += $"{message}\r\n";;
            }

            if(evaluationResult.Diagnostics.Any())
            {
                foreach (var diagnostic in diagnostics)
                {
                    TextOutput += $"{diagnostic}\r\n";

                    var error = syntaxTree.Text.ToString();

                    TextOutput += $"{error}\r\n";
                }

            }
        }
    }
}
