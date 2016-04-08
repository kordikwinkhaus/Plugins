namespace Ctor.Views
{
    internal interface ILineHighlightParser
    {
        /// <summary>
        /// Parsuje řádek a určí pozice od - do pro zvýraznění řádku.
        /// </summary>
        LineHighlightPositions GetHighlightPositions(string line);
    }

    public struct LineHighlightPositions
    {
        private int _startOffset;
        private int _endOffset;

        public LineHighlightPositions(int startOffset, int endOffset)
        {
            _startOffset = startOffset;
            _endOffset = endOffset;
        }

        public int StartOffset
        {
            get { return _startOffset; }
        }

        public int EndOffset
        {
            get { return _endOffset; }
        }
    }
}
