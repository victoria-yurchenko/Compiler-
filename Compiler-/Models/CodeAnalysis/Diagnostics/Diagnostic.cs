using System;
using System.Collections;
using System.Text;
using Compiler.CodeAnalysis.Text;

namespace Compiler.CodeAnalysis.Diagnostics
{
    public class Diagnostic
    {
        public Diagnostic(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public override string ToString()
        {
            return $"{Message}";
        }
    }
}
