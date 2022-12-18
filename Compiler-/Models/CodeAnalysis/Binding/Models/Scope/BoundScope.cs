using Compiler_.Models.CodeAnalysis.Symbols;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.CodeAnalysis.Binding.Models.Scope
{
    public sealed class BoundScope
    {
        private Dictionary<string, Variable> _variables;
        private Dictionary<string, Function> _functions;

        public BoundScope(BoundScope parent)
        {
            _variables = new Dictionary<string, Variable>();
            _functions = new Dictionary<string, Function>();
            Parent = parent;
        }

        public BoundScope Parent { get; }

        public bool TryDeclareVariable(Variable variable)
        {
            if (_variables.ContainsKey(variable.Name))
                return false;

            _variables.Add(variable.Name, variable);
            return true;
        }

        public bool TryLookUpVariable(string name, out Variable variable)
        {
            if (_variables.TryGetValue(name, out variable))
                return true;

            if (Parent == null)
                return false;

            return Parent.TryLookUpVariable(name, out variable);
        }

        public IReadOnlyList<Variable> GetDeclaredVariables()
        {
            return _variables.Values.ToList();
        }


        public bool TryDeclareFunction(Function function)
        {
            if (_variables.ContainsKey(function.Name))
                return false;

            _functions.Add(function.Name, function);
            return true;
        }

        public bool TryLookUpFunction(string name, out Function function)
        {
            if (_functions.TryGetValue(name, out function))
                return true;

            if (Parent == null)
                return false;

            return Parent.TryLookUpFunction(name, out function);
        }

        public IReadOnlyList<Function> GetDeclaredFunctions()
        {
            return _functions.Values.ToList();
        }
    }
}
