using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Text;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata;
using System.Windows.Media.TextFormatting;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Compiler.CodeAnalysis.Diagnostics
{
    public sealed class DiagnosticList : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics;

        public DiagnosticList()
        {
            _diagnostics = new List<Diagnostic>();
        }

        public DiagnosticList(List<Diagnostic> diagnostics)
        {
            _diagnostics = diagnostics;
        }

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void ReportWrongSymbol(char symbol)
        {
            var message = $"Bad character input: <{symbol}>.";
            ReportError(message);
        }

        internal void ReportInvalidLiteral(string text, SMType type)
        {
            var message = $"The literal <{text}> is not valid for the type <{type}>.";
            ReportError(message);
        }

        private void ReportError(string message)
        {
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void AddRange(DiagnosticList diagnostic)
        {
            _diagnostics.AddRange(diagnostic._diagnostics);
        }

        internal void ReportUnexpectedToken(SyntaxKind currentKind, SyntaxKind expectedKind)
        {
            var message = $"Unexpected token <{currentKind}>, expected <{expectedKind}>.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportUndefinedUnaryOperator(string text, SMType type)
        {
            var message = $"Unary operator <{text}> is not defined for the type <{type}>.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportUndefinedBinaryOperator(string text, SMType typeLeft, SMType typeRigth)
        {
            var message = $"Binary operator <{text}> is not defined for the types <{typeLeft}> and <{typeRigth}>.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportUndefinedVariable(string name)
        {
            var message = $"Variable <{name}> does not exist.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportVariableAlreadyDeclared(string name)
        {
            var message = $"Variable <{name}> is already exist.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportCannotConvert(SMType fromType, SMType toType)
        {
            var message = $"Cannot convert type <{fromType}> to type <{toType}>.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportCannotAssign(string name)
        {
            var message = $"Variable <{name}> is a read-only constant and cannot be assigned.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportUnterminatedString()
        {
            var message = $"Unterminated string";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportUndefinedFunction(string name)
        {
            var message = $"Function <{name}> does not exist.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportWrongArgumentCount(string name, int expectedCount, int currentCount)
        {
            var message = $"Wrong argument of function <{name}> requires <{expectedCount}> actual count <{currentCount}>.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportWrongArgumentType(string name, SMType expectedType, SMType currentType)
        {
            var message = $"Parameter <{name}> requires a value of type <{expectedType}> but was given a value of type <{currentType}>.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportExpressionMustHaveValue()
        {
            var message = $"Expression must have a value.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportUndefinedType(string text)
        {
            var message = $"Type <{text}> does not exist.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportParameterAlreadyDeclared(string name)
        {
            var message = $"Parameter <{name}> is already declared.";
            _diagnostics.Add(new Diagnostic(message));
        }

        internal void ReportFunctionAlreadyExist(string name)
        {
            var message = $"Function <{name}> is already exist.";
            _diagnostics.Add(new Diagnostic(message));
        }
    }
}
