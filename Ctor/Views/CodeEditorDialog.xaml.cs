using System;
using System.ComponentModel;
using System.Linq;
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

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            UpdateColumnsWidth(tlvLocals);
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

        private void TreeListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnsWidth(tlvLocals);
        }

        private void UpdateColumnsWidth(TreeListView treeListView)
        {
            int[] ratios = new int[] { 1, 2, 1 };
            double total = ratios.Sum();
            double availableSpace = treeListView.ActualWidth;
            double pxPerRatio = availableSpace / total;

            for (int i = 0; i < ratios.Length - 1; i++)
            {
                double colWidth = ratios[i] * pxPerRatio;
                availableSpace -= colWidth;
                treeListView.Columns[i].Width = colWidth;
            }
            treeListView.Columns.Last().Width = availableSpace;
        }
    }
}
