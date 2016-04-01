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
        private readonly ISqlConnectionWrapper _conn;
        private readonly PythonScriptEngine _engine;

        public FastInsertViewModel(string connection, string lang)
        {
            _connection = connection;
            _lang = lang;

            _conn = new SqlConnectionWrapper(Okna.Data.Utils.ModifyConnString(connection));
            _conn.TrySetLang();

            _engine = new PythonScriptEngine();

            this.RunCommand = new RelayCommand(Run);
            this.Context = new ContextViewModel(this);
        }

        #region Contextual info

        private IOknaDocument _document;
        internal IOknaDocument Document
        {
            get { return _document; }
            set
            {
                if (_document != value)
                {
                    _document = value;
                    if (_database != null)
                    {
                        _database.CurrentDocument = _document;
                    }
                }
            }
        }

        internal IOknaApplication Application
        {
            get { return this.Document?.Application; }
        }

        private Database _database;
        internal IDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new Database(_conn, this.Application);
                    _database.CurrentDocument = this.Document;
                }
                return _database;
            }
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
            _engine.Variables.SetVariable("db", this.Database);

            try
            {
                if (!_engine.Execute(code))
                {
                    msg.Error(_engine.ErrorMessage);
                }
            }
            catch (CompilationException ex)
            {
                var inner = ex.InnerException;
                var syntaxError = inner as SyntaxErrorException;
                if (syntaxError != null)
                {
                    msg.Error("Syntax error at line " + syntaxError.Line + ", column " + syntaxError.Column);
                }
                else
                {
                    msg.Error(inner.Message);
                }
            }
        }

        #endregion
    }
}
