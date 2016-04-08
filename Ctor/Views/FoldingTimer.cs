using System;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;

namespace Ctor.Views
{
    internal class FoldingTimer
    {
        private readonly TextEditor _editor;
        private readonly FoldingManager _manager;
        private readonly PythonFoldingStrategy _strategy;
        private DispatcherTimer _scriptTimer;

        internal FoldingTimer(TextEditor editor, FoldingManager manager, PythonFoldingStrategy strategy)
        {
            _editor = editor;
            _manager = manager;
            _strategy = strategy;

            editor.IsVisibleChanged += (sender, e) =>
            {
                if (!editor.IsVisible && _scriptTimer != null)
                {
                    StopScriptTimer();
                }
            };
            editor.TextChanged += (sender, e) => SetScriptTimer();
        }

        void SetScriptTimer()
        {
            if (_scriptTimer != null)
            {
                _scriptTimer.Stop();
            }
            _scriptTimer = new DispatcherTimer();
            _scriptTimer.Interval = TimeSpan.FromSeconds(2);
            _scriptTimer.Tick += (sender, e) =>
            {
                //_strategy.UpdateFoldings(_manager, _editor.Document);
                _manager.UpdateFoldings(_strategy.CreateNewFoldings(_editor.Document), -1);
                StopScriptTimer();
            };
            _scriptTimer.Start();
        }

        void StopScriptTimer()
        {
            _scriptTimer.Stop();
            _scriptTimer = null;
        }
    }
}