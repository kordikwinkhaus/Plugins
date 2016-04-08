using System;
using System.Windows;

namespace Ctor.Views
{
    public partial class CodeEditorDialog : Window
    {
        public CodeEditorDialog()
        {
            InitializeComponent();
            codeEditor.TextArea.Caret.PositionChanged += new EventHandler(Caret_PositionChanged);
        }

        void Caret_PositionChanged(object sender, EventArgs e)
        {
            var caret = codeEditor.TextArea.Caret;
            lblLineNo.Content = caret.Line.ToString();
            lblColNo.Content = caret.Column.ToString();
        }

        public PythonEditor Editor
        {
            get { return codeEditor; }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
