using Compiler.CodeAnalysis.Binding;
using Compiler.CodeAnalysis.Binding.Models;
using Compiler.CodeAnalysis.Binding.Models.BoundExpressions;
using Compiler.CodeAnalysis.Binding.Models.Scope;
using Compiler.CodeAnalysis.Diagnostics;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler_.Models.CodeAnalysis.Lowering;
using Compiler_.Models.CodeAnalysis.Symbols;
using Compler.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Binder = Compiler.CodeAnalysis.Binding.Binder;

namespace Compiler.CodeAnalysis.Compilation
{
    public sealed class Compilation
    {
        private BoundGlobalScope _globalScope;

        public Compilation(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public Compilation Previous { get; }
        public SyntaxTree SyntaxTree { get; }

        public BoundGlobalScope GlobalScope
        {
            get
            {
                if (_globalScope == null)
                {
                    var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, SyntaxTree.Root);
                    Interlocked.CompareExchange(ref _globalScope, globalScope, null);
                }

                return _globalScope;
            }
        }

        public EvaluationResult Evaluate(Dictionary<Variable, object> variables)
        {
            var diagnostic = SyntaxTree.Diagnostics.Concat(GlobalScope.Diagnostics).ToList();

            if (diagnostic.Any())
                return new EvaluationResult(diagnostic, null);

            var program = Binder.BindProgram(GlobalScope);
            if(program.Diagnostics.Any())
                return new EvaluationResult(program.Diagnostics.ToList(), null);

            var evaluator = new Evaluator(program, variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(new List<Diagnostic>(), value);
        }
    }
}