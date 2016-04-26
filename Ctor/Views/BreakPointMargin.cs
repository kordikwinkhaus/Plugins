using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;

namespace Ctor.Views
{
    internal class BreakPointMargin : AbstractMargin
    {
        private const int _margin = 18;
        private const int _radius = _margin / 2 - 4;
        private IList<int> _breakpoints;

        public BreakPointMargin(IList<int> breakpoints)
        {
            MouseDown += new MouseButtonEventHandler(BreakPointMargin_MouseDown);
            _breakpoints = breakpoints;
        }

        public IList<int> Breakpoints
        {
            get { return _breakpoints; }
        }

        protected override void OnDocumentChanged(TextDocument oldDocument, TextDocument newDocument)
        {
            base.OnDocumentChanged(oldDocument, newDocument);
            _breakpoints.Clear();
            InvalidateVisual();
            var tracker = new LineTracker(ln => RecalculateBreakpoints(ln, 1), ln => RecalculateBreakpoints(ln, -1));
            newDocument.LineTrackers.Add(tracker);
        }

        private void RecalculateBreakpoints(int from, int step)
        {
            for (int i = _breakpoints.Count - 1; i >= 0; i--)
            {
                if (_breakpoints[i] == from && step == -1)
                {
                    _breakpoints.RemoveAt(i);
                }
                else if (_breakpoints[i] >= from)
                {
                    _breakpoints[i] += step;
                }
            }
        }

        protected override void OnTextViewChanged(TextView oldTextView, TextView newTextView)
        {
            base.OnTextViewChanged(oldTextView, newTextView);
            InvalidateVisual();
            newTextView.ScrollOffsetChanged += new EventHandler(newTextView_ScrollOffsetChanged);
            newTextView.VisualLinesChanged += new EventHandler(newTextView_VisualLinesChanged);
        }

        private void newTextView_VisualLinesChanged(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        private void newTextView_ScrollOffsetChanged(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(_margin, 0);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var tv = this.TextView;
            Size renderSize = this.RenderSize;

            if (tv != null)
            {
                foreach (int bpLine in _breakpoints)
                {
                    var visualLine = tv.GetVisualLine(bpLine);

                    if (visualLine != null && visualLine.FirstDocumentLine.LineNumber == bpLine)
                    {
                        drawingContext.DrawEllipse(Brushes.Tomato,
                            new Pen(Brushes.DarkRed, 1),
                            new Point(_margin / 2, visualLine.VisualTop - tv.VerticalOffset + (_margin / 2) - 1),
                            _radius, _radius);
                    }
                }
            }
        }

        private void BreakPointMargin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(this);
                var docLine = this.TextView.GetDocumentLineByVisualTop(pos.Y + this.TextView.VerticalOffset);

                int lineNumber = docLine.LineNumber;
                if (_breakpoints.Contains(lineNumber))
                {
                    _breakpoints.Remove(lineNumber);
                }
                else if (this.Document.GetText(docLine).Trim().Length != 0) // don't add breakpoints to empty lines
                {
                    _breakpoints.Add(lineNumber);
                }
                this.InvalidateVisual();
            }
        }
    }
}