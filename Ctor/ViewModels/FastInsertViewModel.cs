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

            var sqlConn = new SqlConnectionWrapper(Okna.Data.Utils.ModifyConnString(connection));
            sqlConn.TrySetLang();
            _repository = new Repository(sqlConn);

            _engine = new PythonScriptEngine();

            this.RunCommand = new RelayCommand(Run);
            this.Context = new ContextViewModel(this);
        }

        #region Contextual info

        internal IOknaDocument Document { get; set; }

        internal IOknaApplication Application
        {
            get { return this.Document?.Application; }
        }

        private IDocumentsDialogFactory _dialogFactory;
        internal IDocumentsDialogFactory DialogFactory
        {
            get
            {
                if (_dialogFactory == null)
                {
                    var oknaApp = this.Document.Application;
                    if (oknaApp != null)
                    {
                        _dialogFactory = new DocumentsDialogFactory();
                        _dialogFactory.Init(oknaApp, _connection);
                    }
                }
                return _dialogFactory;
            }
        }

        public ContextViewModel Context { get; private set; }

        #endregion

        #region Script invocation

        public ICommand RunCommand { get; private set; }

        private void Run(object param)
        {
            string code = (string)param;

            _engine.InitVariablesScope();
            _engine.Variables.SetVariable("pos", new Position(this.Document.ActivePos));
            _engine.Variables.SetVariable("ctx", new Context(this.Context));
            var msg = new Msg();
            _engine.Variables.SetVariable("msg", msg);

            try
            {
                if (!_engine.Execute(code))
                {
                    msg.error(_engine.ErrorMessage);
                }
            }
            catch (CompilationException ex)
            {
                var inner = ex.InnerException;
                var syntaxError = inner as SyntaxErrorException;
                if (syntaxError != null)
                {
                    msg.error("Syntax error at line " + syntaxError.Line + ", column " + syntaxError.Column);
                }
                else
                {
                    msg.error(inner.Message);
                }
            }
        }

        #endregion
    }
}
