using Compiler.CodeAnalysis.Diagnostics;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Compiler.CodeAnalysis.Text
{
    public sealed class SourceText
    {
        public List<TextLine> TextLines { get; }
        public string Text { get; }

        public SourceText(string text)
        {
            TextLines = ParseLines(this, text);
            Text = text;
        }

        public char this[int position] => Text[position];
        public int Length => Text.Length;

        public int GetLineIndex(int position)
        {
            var lowerBound = 0;
            var upperBound = TextLines.Count - 1;

            while (lowerBound <= upperBound)
            {
                int index = lowerBound + (upperBound - lowerBound) / 2;
                var start = TextLines.ToArray()[index].StartFrom;

                if (position == start)
                    return index;

                if (position < start)
                    upperBound = index - 1;
                else
                    lowerBound = index + 1;
            }

            return lowerBound - 1;
        }

        private List<TextLine> ParseLines(SourceText sourceText, string text)
        {
            var result = new List<TextLine>();
            var lineStart = 0;
            var currentPosition = 0;

            while (currentPosition < text.Length)
            {
                var lineBreakWidth = GetLineBreakWidth(text, currentPosition);

                if (lineBreakWidth == 0)
                {
                    currentPosition++;
                }
                else
                {
                    AddLine(sourceText, result, lineStart, currentPosition, lineBreakWidth);

                    currentPosition += lineBreakWidth;
                    lineStart = currentPosition;
                }
            }

            if (currentPosition >= lineStart)
            {
                AddLine(sourceText, result, lineStart, currentPosition, 0);
            }

            return result;
        }

        private static void AddLine(SourceText sourceText, List<TextLine> result, int lineStart, int currentPosition, int lineBreakWidth)
        {
            var lineLength = currentPosition - lineStart;
            var lineLengthWithLineBreak = lineLength + lineBreakWidth;
            var line = new TextLine(sourceText, lineStart, lineLength, lineLengthWithLineBreak);
            result.Add(line);
        }

        private int GetLineBreakWidth(string text, int position)
        {
            var symbol = text[position];
            var lookAhead = position + 1 >= text.Length ? '\0' : text[position + 1];

            if (symbol == '\r' && lookAhead == '\n')
                return 2;

            if (symbol == '\r' || symbol == '\n')
                return 1;

            return 0;

        }

        public static SourceText From(string text)
        {
            return new SourceText(text);
        }

        public string ToString(int start, int length)
        {
            return Text.Substring(start, length);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
