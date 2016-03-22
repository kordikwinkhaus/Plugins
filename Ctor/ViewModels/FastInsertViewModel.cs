using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Okna.Documents;
using Okna.Plugins.ViewModels;
using WHOkna;

namespace Ctor.ViewModels
{
    public class FastInsertViewModel : ViewModelBase
    {
        private readonly string _connection;
        private readonly string _lang;

        public FastInsertViewModel(string connection, string lang)
        {
            _connection = connection;
            _lang = lang;
        }

        internal IOknaDocument Document { get; set; }

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
