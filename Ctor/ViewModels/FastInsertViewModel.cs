using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Ctor.Models;
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

            this.RunCommand = new RelayCommand(Run);
        }

        internal IOknaDocument Document { get; set; }

        public ICommand RunCommand { get; private set; }

        private void Run(object param)
        {
            var pos = new Position(this.Document.ActivePos);
            //pos.Clear();
            pos.InsertFrames(109, 253);
            var frame = pos.GetFrame(1);
            //frame.InsertSashes();
            frame.InsertGlasspackets("PLU4/12/P4/12/PLU4");
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
