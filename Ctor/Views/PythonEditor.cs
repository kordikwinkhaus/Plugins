using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using Ctor.Models.Scripting;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Rendering;

namespace Ctor.Views
{
    public class PythonEditor : TextEditor, IScriptEditor
    {
        private IList<int> _breakpoints;
        private HighlightLineBackgroundRenderer _lineHighlighter;

        public PythonEditor()
        {
            FontFamily = new FontFamily("Consolas");
            FontSize = 14;
            ShowLineNumbers = true;
            Options.IndentationSize = 4;
            Options.ConvertTabsToSpaces = true;

            InitSyntaxHighlighting();
            InitBreakPointHandling();
            InitFolding();

            this.TextArea.IndentationStrategy = new PythonIndentationStrategy();
        }

        private void InitSyntaxHighlighting()
        {
            using (System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Ctor.PythonHighlight.xshd"))
            {
                this.SyntaxHighlighting = HighlightingLoader.Load(new XmlTextReader(s), HighlightingManager.Instance);
            }
        }

        private void InitBreakPointHandling()
        {
            _breakpoints = new List<int>();
            var bpMargin = new BreakPointMargin(_breakpoints);
            this.TextArea.LeftMargins.Insert(0, bpMargin);
        }

        private void InitFolding()
        {
            // add rectangle as background for folding margin
            var foldingBackground = new Rectangle();
            foldingBackground.Height = 5000;
            foldingBackground.Width = 21;
            foldingBackground.Margin = new Thickness(-2, 0, -19, 0);
            foldingBackground.Fill = Brushes.WhiteSmoke;
            this.TextArea.LeftMargins.Add(foldingBackground);

            var manager = FoldingManager.Install(this.TextArea);
            var strategy = new PythonFoldingStrategy();
            var timer = new FoldingTimer(this, manager, strategy);
        }

        public void BeginScriptExecMode()
        {
            SetState(false);
            if (_lineHighlighter == null)
            {
                _lineHighlighter = new HighlightLineBackgroundRenderer(this, new PythonLineHighlightParser());
            }
            this.TextArea.TextView.BackgroundRenderers.Add(_lineHighlighter);
        }

        public void EndScriptExecMode()
        {
            SetState(true);
            this.TextArea.TextView.BackgroundRenderers.Remove(_lineHighlighter);
        }

        private void SetState(bool enabled)
        {
            this.IsReadOnly = !enabled;
            this.Background = (enabled) ? Brushes.White : Brushes.WhiteSmoke;
        }

        public void HighlightLine(int? line, SolidColorBrush background)
        {
            _lineHighlighter.LineNumber = line;
            _lineHighlighter.BackgroundBrush = background;

            if (line.HasValue)
            {
                this.ScrollToLine(line.Value);
            }

            this.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
        }

        public bool IsBreakPointOnLine(int line)
        {
            return _breakpoints.Contains(line);
        }
    }
}
