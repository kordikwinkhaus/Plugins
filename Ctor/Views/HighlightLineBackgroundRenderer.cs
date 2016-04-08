using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;

namespace Ctor.Views
{
    internal class HighlightLineBackgroundRenderer : IBackgroundRenderer
    {
        private const double _radius = 3;
        private readonly TextEditor _editor;
        private readonly ILineHighlightParser _parser;

        public HighlightLineBackgroundRenderer(TextEditor editor, ILineHighlightParser parser)
        {
            _editor = editor;
            _parser = parser;
        }

        public int? LineNumber { get; set; }

        public SolidColorBrush BackgroundBrush { get; set; }

        #region IBackgroundRenderer Members

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_editor.Document == null) return;
            if (!LineNumber.HasValue) return;

            textView.EnsureVisualLines();
            var line = _editor.Document.GetLineByNumber(LineNumber.Value);

            int offset = _editor.Document.GetOffset(line.LineNumber, 0);
            string text = _editor.Document.GetText(offset, line.Length);
            var pos = _parser.GetHighlightPositions(text);

            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, line))
            {
                double charWidth = rect.Width / line.Length;
                Rect justTextRect = new Rect();
                justTextRect.Y = rect.Y;
                justTextRect.X = rect.X + (pos.StartOffset * charWidth);
                justTextRect.Height = rect.Height;
                justTextRect.Width = rect.Width - ((pos.StartOffset + pos.EndOffset) * charWidth);
                drawingContext.DrawRoundedRectangle(BackgroundBrush, null, justTextRect, _radius, _radius);
            }
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Background; }
        }

        #endregion
    }
}
