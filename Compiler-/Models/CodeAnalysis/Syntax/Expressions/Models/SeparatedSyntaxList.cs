using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using System.Collections;
using System.Collections.Generic;

namespace Compiler.CodeAnalysis.Syntax.Expressions.Models
{
    public abstract class SeparatedSyntaxList
    {
        public abstract List<Node> GetSeparatorsAndNodes();
    }

    public sealed class SeparatedSyntaxList<T> : SeparatedSyntaxList, IEnumerable<T>
           where T : Node
    {
        private readonly List<Node> _separatorsAndNodes;

        public SeparatedSyntaxList(List<Node> separatorsAndNodes)
        {
            _separatorsAndNodes = separatorsAndNodes;
        }

        public int Count => (_separatorsAndNodes.Count + 1) / 2;
        public T this[int index] => (T)_separatorsAndNodes[index* 2];

        public override List<Node> GetSeparatorsAndNodes() => _separatorsAndNodes;

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}