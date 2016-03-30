using System.Windows.Input;
using Ctor.Models;
using Microsoft.Scripting;
using Okna.Data;
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
        private readonly IRepository _repository;

        public FastInsertViewModel(string connection, string lang)
        {
            _connection = connection;
            _lang = lang;

            var sqlConn = new SqlConnectionWrapper(Utils.ModifyConnString(connection));
            sqlConn.TrySetLang();
            _repository = new Repository(sqlConn);

            _engine = new PythonScriptEngine();

            this.RunCommand = new RelayCommand(Run);
        }

        internal IOknaDocument Document { get; set; }

        public ICommand RunCommand { get; private set; }

        private void Run(object param)
        {
            string code = (string)param;

            _engine.InitVariablesScope();
            _engine.Variables.SetVariable("pos", new Position(this.Document.ActivePos));
            _engine.Variables.SetVariable("ctx", new Context(this));
            _engine.Variables.SetVariable("msg", new Msg());

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
                        //_dialogFactory = DocumentsDialogFactory.Instance;
                        //_dialogFactory.Init(oknaApp, _connection);
                    }
                }
                return _dialogFactory;
            }
        }
    }
}
