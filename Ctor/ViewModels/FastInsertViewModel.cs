using System.Windows.Input;
using Ctor.Models;
using Ctor.Models.Scripting;
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
        private PythonScriptEngine _engine;
        private ContextScriptScopeExtender _scopeExtender;

        public FastInsertViewModel(string connection, string lang)
        {
            _connection = connection;
            _lang = lang;

            _conn = new SqlConnectionWrapper(Okna.Data.Utils.ModifyConnString(connection));
            _conn.TrySetLang();
            
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
            var engine = GetScriptEngine();

            try
            {
                if (!engine.Execute(code))
                {
                    Msg.Instance.Error(engine.ErrorMessage);
                }
            }
            catch (CompilationException ex)
            {
                var inner = ex.InnerException;
                var syntaxError = inner as SyntaxErrorException;
                if (syntaxError != null)
                {
                    Msg.Instance.Error("Syntax error at line " + syntaxError.Line + ", column " + syntaxError.Column);
                }
                else
                {
                    Msg.Instance.Error(inner.Message);
                }
            }
        }

        internal PythonScriptEngine GetScriptEngine()
        {
            if (_engine == null)
            {
                _engine = new PythonScriptEngine();
                _scopeExtender = new ContextScriptScopeExtender(this);
            }

            _engine.InitVariablesScope();
            _scopeExtender.ExtendScope(_engine.Variables);

            return _engine;
        }

        #endregion
    }
}
