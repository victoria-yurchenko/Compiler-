using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CodeAnalysis.Syntax.TreeStructure.Models
{
    public abstract class Node
    {
        public abstract SyntaxKind SyntaxKind { get; }
    }
}
