namespace Compiler.CodeAnalysis.Text
{
    public sealed class TextLine
    {
        public TextLine(SourceText text, int startFrom, int length, int lengthIncludingLineBreak)
        {
            Text = text;
            StartFrom = startFrom;
            Length = length;
            LengthIncludingLineBreak = lengthIncludingLineBreak;
        }

        public SourceText Text { get; }
        public int StartFrom { get; }
        public int Length { get; }
        public int End => StartFrom + Length;
        public int LengthIncludingLineBreak { get; }
    }
}
