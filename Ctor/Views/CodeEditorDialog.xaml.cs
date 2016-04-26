using System;
using System.ComponentModel;
using System.Windows;
using Ctor.Models;
using Ctor.Models.Scripting;
using Ctor.Resources;
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

        public bool SaveChanges { get; private set; }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.SaveChanges = false;
            this.Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.SaveChanges = true;
            this.Close();
        }

        private void txtOutput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            txtOutput.ScrollToEnd();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            var vm = (CodeEditorViewModel)this.DataContext;
            if (vm.ExecutingScript)
            {
                MessageBox.Show(Strings.RunningScript, Msg.CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }
    }
}
