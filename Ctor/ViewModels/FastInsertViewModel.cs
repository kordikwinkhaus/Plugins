using System.Windows;
using System.Windows.Input;
using System.Windows.ViewModels;
using Ctor.Models;
using Ctor.Models.Scripting;
using Ctor.Views;
using Microsoft.Scripting;
using Okna.Data;
using Okna.Documents;
using WHOkna;

namespace Ctor.ViewModels
{
    public class FastInsertViewModel : ViewModelBase
    {
        private readonly string _connection;
        private readonly string _lang;
        private readonly IInteractionService _interaction;
        private readonly ISqlConnectionWrapper _conn;
        private PythonScriptEngine _engine;
        private ContextScriptScopeExtender _scopeExtender;

        internal FastInsertViewModel(string connection, string lang, IInteractionService interaction)
        {
            _connection = connection;
            _lang = lang;
            _interaction = interaction;

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
        private InteractionService interaction;

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
            AreaSelector.SelectArea = SelectArea;

            try
            {
                if (!engine.Execute(code))
                {
                    _interaction.ShowError(engine.ErrorMessage, Msg.CAPTION);
                }
            }
            catch (CompilationException ex)
            {
                var inner = ex.InnerException;
                var syntaxError = inner as SyntaxErrorException;
                if (syntaxError != null)
                {
                    _interaction.ShowError("Syntax error at line " + syntaxError.Line + ", column " + syntaxError.Column, Msg.CAPTION);
                }
                else
                {
                    _interaction.ShowError(inner.Message, Msg.CAPTION);
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

        internal IArea SelectArea(IAreaProvider areaProvider)
        {
            var areaSelectorVM = new AreaSelectorViewModel(areaProvider);
            
            if (_interaction.ShowDialogCenteredToMouse(areaSelectorVM) == true)
            {
                return areaSelectorVM.SelectedArea;
            }
            else
            {
                throw new IronPython.Runtime.Exceptions.SystemExitException();
            }
        }

        #endregion
    }
}
