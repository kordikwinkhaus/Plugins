using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.ViewModels;
using Ctor.Models.Scripting;
using Ctor.Resources;

namespace Ctor.ViewModels
{
    public class CodeEditorViewModel : ViewModelBase
    {
        private readonly PythonScriptRunner _runner;
        private readonly TextOutputStream _output;

        internal CodeEditorViewModel(IScriptEditor scriptEditor, FastInsertViewModel parent, TaskScheduler scheduler)
        {
            this.CompileCommand = new RelayCommand(Compile, CanCompile);
            this.RunScriptCommand = new RelayCommand(RunScript, CanRunScript);
            this.DebugScriptCommand = new RelayCommand(DebugScript, CanDebugScript);
            this.StepIntoCommand = new RelayCommand(StepInto, CanDebugStep);
            this.StepOutCommand = new RelayCommand(StepOut, CanDebugStep);
            this.StepOverCommand = new RelayCommand(StepOver, CanDebugStep);
            this.RunToEndCommand = new RelayCommand(RunToEnd, CanDebugStep);
            this.RunToBreakPointCommand = new RelayCommand(RunToBreakPoint, CanDebugStep);
            this.StopDebugCommand = new RelayCommand(StopDebug, CanStopDebug);
            this.ClearOutputCommand = new RelayCommand(ClearOutput);

            this.DebugInfo = Strings.Ready;

            _output = new TextOutputStream();
            _output.TextChanged += output_TextChanged;

            _runner = new PythonScriptRunner(scriptEditor, _output, parent.GetScriptEngine(), scheduler);
            _runner.ScriptFinished += new EventHandler(ScriptFinished);
            _runner.DebugInfoChanged += new EventHandler(DebugInfoChanged);
        }

        private void output_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.OutputText = e.Text;
        }

        private string _outputText;
        public string OutputText
        {
            get { return _outputText; }
            set
            {
                if (_outputText != value)
                {
                    _outputText = value;
                    OnPropertyChanged(nameof(OutputText));
                }
            }
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

        public ICommand CompileCommand { get; private set; }
        public ICommand RunScriptCommand { get; private set; }
        public ICommand DebugScriptCommand { get; private set; }
        public ICommand StepIntoCommand { get; private set; }
        public ICommand StepOutCommand { get; private set; }
        public ICommand StepOverCommand { get; private set; }
        public ICommand RunToEndCommand { get; private set; }
        public ICommand RunToBreakPointCommand { get; private set; }
        public ICommand StopDebugCommand { get; private set; }
        public ICommand ClearOutputCommand { get; private set; }

        private void Compile(object param)
        {
            if (this.CanCompile(param))
            {
                StopTimer();

                _runner.Compile();
                this.DebugInfo = Strings.SuccessfullyCompiled;

                SetReadyTimer();
            }
        }

        private bool CanCompile(object param)
        {
            return _canRunScript;
        }

        private void RunScript(object param)
        {
            StopTimer();

            _runner.Run();
            _canDebugScript = false;
        }

        private bool CanRunScript(object param)
        {
            return _canRunScript;
        }

        private void DebugScript(object param)
        {
            StopTimer();

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

        private bool CanStopDebug(object param)
        {
            return false;
        }

        private void StopDebug(object param)
        {
            if (this.CanStopDebug(param))
            {

            }
        }

        private void ClearOutput(object param)
        {
            _output.Clear();
        }

        #endregion
    }
}
