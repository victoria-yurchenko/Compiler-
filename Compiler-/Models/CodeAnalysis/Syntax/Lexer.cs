using Compiler.CodeAnalysis.Diagnostics;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler.CodeAnalysis.Text;
using Compiler_.Models.CodeAnalysis.Symbols;
using System;
using System.Text;

namespace Compiler.CodeAnalysis.Syntax
{
    public class Lexer
    {
        private readonly SourceText _text;
        private readonly DiagnosticList _diagnostic;

        private int _position;
        private int _start;
        private SyntaxKind _kind;
        private object _value;

        public DiagnosticList Diagnostic => _diagnostic;
        private char Current => Peek(0);
        private char LookAhead => Peek(1);

        public Lexer(SourceText text)
        {
            _text = text;
            _diagnostic = new DiagnosticList();
        }

        public Token GetNextToken()
        {
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;

            switch (Current)
            {
                case '\0':
                    _kind = SyntaxKind.EndOfFileToken;
                    break;

                case '+':
                    _kind = SyntaxKind.PlusToken;
                    _position++;
                    break;

                case '-':
                    _kind = SyntaxKind.MinusToken;
                    _position++;
                    break;

                case '*':
                    _kind = SyntaxKind.StarToken;
                    _position++;
                    break;
                case '/':
                    _kind = SyntaxKind.SlashToken;
                    _position++;
                    break;

                case '(':
                    _kind = SyntaxKind.OpenBracketToken;
                    _position++;
                    break;

                case ')':
                    _kind = SyntaxKind.CloseBracketToken;
                    _position++;
                    break;

                case '{':
                    _kind = SyntaxKind.OpenBraceToken;
                    _position++;
                    break;

                case '}':
                    _kind = SyntaxKind.CloseBraceToken;
                    _position++;
                    break;

                case ',':
                    _kind = SyntaxKind.CommaToken;
                    _position++;
                    break;

                case ':':
                    _kind = SyntaxKind.ColonToken;
                    _position++;
                    break;

                case '!':
                    {
                        _position++;
                        if (Current != '=')
                            _kind = SyntaxKind.LogicalNotToken;
                        else
                        {
                            _kind = SyntaxKind.LogicalNotEqualsToken;
                            _position++;
                        }
                        break;
                    }

                case '&':
                    if (_text.Length > 1 && LookAhead == '&')
                    {
                        _kind = SyntaxKind.EndOfFileToken;
                        _position += 2;
                        break;
                    }
                    break;

                case '|':
                    if (_text.Length > 1 && LookAhead == '|')
                    {
                        _kind = SyntaxKind.LogicalOrToken;
                        _position += 2;
                        break;
                    }
                    break;

                case '=':
                    {
                        _position++;
                        if (Current != '=')
                            _kind = SyntaxKind.EqualsToken;
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.LogicalEqualsToken;
                        }
                        break;
                    }

                case '<':
                    {
                        _position++;
                        if (Current != '=')
                            _kind = SyntaxKind.LessToken;
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.LessEqualsToken;
                        }
                        break;
                    }

                case '>':
                    {
                        _position++;
                        if (Current != '=')
                            _kind = SyntaxKind.GreaterToken;
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.GreaterEqualsToken;
                        }
                        break;
                    }

                case '"':
                    ReadString();
                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    {
                        ReadNumberToken();
                        break;
                    }

                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    {
                        ReadWhitespace();
                        break;
                    }

                default:
                    {
                        if (char.IsWhiteSpace(Current))
                            ReadWhitespace();
                        else if (char.IsLetter(Current))
                            ReadIdentifierOrKeyword();
                        else
                        {
                            _diagnostic.ReportWrongSymbol(Current);
                            _position++;
                        }
                        break;
                    }
            }

            var length = _position - _start;
            var text = SyntaxAxioms.GetTextFromSyntaxKind(_kind);
            if (text == null)
                text = _text.ToString(_start, length);

            return new Token(_kind, _start, text, _value);
        }

        private void ReadString()
        {
            _position++;

            var stringBuilder = new StringBuilder();
            var done = false;

            while (!done)
            {
                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        _diagnostic.ReportUnterminatedString();
                        done = true;
                        break;

                    case '"':
                        if(LookAhead == '"')
                        {
                            stringBuilder.Append(Current);
                            _position+=2;
                        }
                        else
                        {
                            done = true;
                            _position++;
                        }
                        break;

                    default:
                        stringBuilder.Append(Current);
                        _position++;
                        break;
                }
            }

            _kind = SyntaxKind.StringToken;
            _value = stringBuilder.ToString();
        }

        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current))
                _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            _kind = SyntaxAxioms.GetKeywordSyntaxKind(text);
        }

        private void ReadWhitespace()
        {
            while (char.IsWhiteSpace(Current))
                _position++;

            _kind = SyntaxKind.WhitespaceToken;
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
                _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            if (!int.TryParse(text, out var value))
                _diagnostic.ReportInvalidLiteral(text, SMType.Int);

            _value = value;
            _kind = SyntaxKind.LiteralToken;
        }

        private char Peek(int offset)
        {
            var index = _position + offset;

            if (_position >= _text.Length)
                return '\0';

            return _text[index];
        }
    }
}