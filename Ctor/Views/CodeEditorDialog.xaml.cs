using System;
using System.Windows;
using Ctor.Models.Scripting;
using Ctor.ViewModels;

namespace Ctor.Views
{
    public partial class CodeEditorDialog : Window, ICodeEditorView
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

        public IScriptEditor Editor
        {
            get { return codeEditor; }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void txtOutput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            txtOutput.ScrollToEnd();
        }
    }
}
