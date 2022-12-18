using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Binding.Models.BoundExpressions
{
    public class BoundLabel
    {
        public BoundLabel(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
