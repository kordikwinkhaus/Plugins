using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Ctor.Models;
using Microsoft.Scripting;
using Okna.Documents;
using Okna.Plugins.ViewModels;
using WHOkna;

namespace Ctor.ViewModels
{
    public class FastInsertViewModel : ViewModelBase
    {
        private readonly string _connection;
        private readonly string _lang;
        private readonly PythonScriptEngine _engine;

        public FastInsertViewModel(string connection, string lang)
        {
            _connection = connection;
            _lang = lang;

            _engine = new PythonScriptEngine();

            this.RunCommand = new RelayCommand(Run);
        }

        internal IOknaDocument Document { get; set; }

        public ICommand RunCommand { get; private set; }

        private void Run(object param)
        {
            string code = (string)param;

            _engine.Variables.SetVariable("pos", new Position(this.Document.ActivePos));

            try
            {
                if (!_engine.Execute(code))
                {
                    System.Windows.MessageBox.Show(_engine.ErrorMessage);
                }
            }
            catch (CompilationException ex)
            {
                var inner = ex.InnerException;
                var syntaxError = inner as SyntaxErrorException;
                if (syntaxError != null)
                {
                    System.Windows.MessageBox.Show("Line " + syntaxError.Line + ", column " + syntaxError.Column);
                }
                else
                {
                    System.Windows.MessageBox.Show(inner.Message);
                }
            }

            //var pos = new Position(this.Document.ActivePos);
            ////pos.Clear();
            //pos.InsertFrames(109, 253);
            //var frame = pos.GetFrame(1);
            ////frame.InsertSashes();
            //frame.InsertGlasspackets("PLU4/12/P4/12/PLU4");
        }

        private IDocumentsDialogFactory _dialogFactory;
        internal IDocumentsDialogFactory DialogFactory
        {
            get
            {
                if (_dialogFactory == null)
                {
                    var oknaApp = this.Document?.Application;
                    if (oknaApp != null)
                    {
                        _dialogFactory = new DocumentsDialogFactory();
                        _dialogFactory.Init(oknaApp, _connection);
                    }
                }
                return _dialogFactory;
            }
        }
    }
}
