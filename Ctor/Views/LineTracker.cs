using System;
using ICSharpCode.AvalonEdit.Document;

namespace Ctor.Views
{
    internal class LineTracker : ILineTracker
    {
        private readonly Action<int> _lineAdded;
        private readonly Action<int> _lineRemoved;

        public LineTracker(Action<int> lineAdded, Action<int> lineRemoved)
        {
            _lineAdded = lineAdded;
            _lineRemoved = lineRemoved;
        }

        #region ILineTracker Members

        public void BeforeRemoveLine(DocumentLine line)
        {
            _lineRemoved(line.LineNumber);
        }

        public void ChangeComplete(DocumentChangeEventArgs e)
        {
        }

        public void LineInserted(DocumentLine insertionPos, DocumentLine newLine)
        {
            _lineAdded(newLine.LineNumber);
        }

        public void RebuildDocument()
        {
        }

        public void SetLineLength(DocumentLine line, int newTotalLength)
        {
        }

        #endregion
    }
}