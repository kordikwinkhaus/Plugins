using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Ctor.Models.Scripting;
using Ctor.Resources;
using Okna.Plugins.ViewModels;

namespace Ctor.ViewModels
{
    public class CodeEditorViewModel : ViewModelBase
    {
        private readonly PythonScriptRunner _runner;

        internal CodeEditorViewModel(IScriptEditor scriptEditor, FastInsertViewModel parent, TaskScheduler scheduler)
        {
            _runScriptCommand = new RelayCommand(RunScript, CanRunScript);
            _debugScriptCommand = new RelayCommand(DebugScript, CanDebugScript);
            _stepIntoCommand = new RelayCommand(StepInto, CanDebugStep);
            _stepOutCommand = new RelayCommand(StepOut, CanDebugStep);
            _stepOverCommand = new RelayCommand(StepOver, CanDebugStep);
            _runToEndCommand = new RelayCommand(RunToEnd, CanDebugStep);
            _runToBreakPointCommand = new RelayCommand(RunToBreakPoint, CanDebugStep);

            this.DebugInfo = Strings.Ready;

            var _output = new MemoryStream();

            _runner = new PythonScriptRunner(scriptEditor, new StreamWriter(_output), parent.GetScriptEngine(), scheduler);
            _runner.ScriptFinished += new EventHandler(ScriptFinished);
            _runner.DebugInfoChanged += new EventHandler(DebugInfoChanged);
        }

        private string _debugInfo;
        public string DebugInfo
        {
            get { return _debugInfo; }
            set
            {
                if (_debugInfo != value)
                {
                    _debugInfo = value;
                    OnPropertyChanged(nameof(DebugInfo));
                }
            }
        }

        #region Commands for debugging

        private bool _canRunScript = true;
        private bool _canDebugScript = true;
        private bool _canDebugStep = false;

        private ICommand _runScriptCommand;
        public ICommand RunScriptCommand
        {
            get { return _runScriptCommand; }
        }

        private ICommand _debugScriptCommand;
        public ICommand DebugScriptCommand
        {
            get { return _debugScriptCommand; }
        }

        private ICommand _stepIntoCommand;
        public ICommand StepIntoCommand
        {
            get { return _stepIntoCommand; }
        }

        private ICommand _stepOutCommand;
        public ICommand StepOutCommand
        {
            get { return _stepOutCommand; }
        }

        private ICommand _stepOverCommand;
        public ICommand StepOverCommand
        {
            get { return _stepOverCommand; }
        }

        private ICommand _runToEndCommand;
        public ICommand RunToEndCommand
        {
            get { return _runToEndCommand; }
        }

        private ICommand _runToBreakPointCommand;
        public ICommand RunToBreakPointCommand
        {
            get { return _runToBreakPointCommand; }
        }

        private void RunScript(object param)
        {
            _runner.Run();
            _canDebugScript = false;
        }

        private bool CanRunScript(object param)
        {
            return _canRunScript;
        }

        private void DebugScript(object param)
        {
            _canRunScript = false;
            _canDebugScript = false;
            _canDebugStep = true;

            _runner.Debug();
        }

        private bool CanDebugScript(object param)
        {
            return _canDebugScript;
        }

        private bool CanDebugStep(object param)
        {
            return _canDebugStep;
        }

        private void StepInto(object param)
        {
            _runner.StepInto();
        }

        private void StepOut(object param)
        {
            _runner.StepOut();
        }

        private void StepOver(object param)
        {
            _runner.StepOver();
        }

        private void RunToEnd(object param)
        {
            _runner.RunToEnd();
        }

        private void RunToBreakPoint(object param)
        {
            _runner.RunToBreakpoint();
        }

        DispatcherTimer _timer;
        object _lock = new object();

        private void ScriptFinished(object sender, EventArgs e)
        {
            _canRunScript = true;
            _canDebugScript = true;
            _canDebugStep = false;

            SetReadyTimer();
        }

        private void SetReadyTimer()
        {
            lock (_lock)
            {
                StopTimer();
                StartTimer();
            }
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Start();
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lock (_lock)
            {
                StopTimer();
                this.DebugInfo = Strings.Ready;
            }
        }

        private void DebugInfoChanged(object sender, EventArgs e)
        {
            this.DebugInfo = _runner.DebugInfo;
        }

        #endregion
    }
}
