using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using EOkno.Views;
using Okna.Plugins;
using WHOkna;

namespace EOkno.ViewModels
{
    public class PositionViewModel : DocumentViewModel
    {
        private const string s_Inherit = "doc";
        private const string s_True = "1";
        private const string s_False = "0";

        private XElement _data;
        private bool _created;
        private List<XElement> _removedContent;

        internal PositionViewModel()
        {
            this.InheritFromDocument = true;
            this.CopyFromDocumentCommand = new RelayCommand(CopyFromDocument);
        }

        internal IOknaDocument OknaDocument { get; set; }

        public ICommand CopyFromDocumentCommand { get; private set; }

        private void CopyFromDocument(object param)
        {
            DocumentViewModel docVM = null;

            try
            {
                docVM = new DocumentViewModel();
                XElement data = this.OknaDocument.ExtendedProperties;
                XElement eokno = data.Element(DocumentView.s_MainElement);
                if (eokno == null) return;
                UserExt.ExtensionsFactory.Nacist(docVM);
                docVM.SetMainElement(eokno, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (docVM.VybranaPU != null)
            {
                this.VybranaPU = this.PovrchoveUpravy.SingleOrDefault(p => p.Kod == docVM.VybranaPU.Kod);
                if (this.VybranaPU != null)
                {
                    this.VybranaPU.VnejsiOdstin = this.VybranaPU.Odstiny.SingleOrDefault(o => o.Kod == docVM.VybranaPU.VnejsiOdstin?.Kod);
                    this.VybranaPU.VnitrniOdstin = this.VybranaPU.Odstiny.SingleOrDefault(o => o.Kod == docVM.VybranaPU.VnitrniOdstin?.Kod);
                }
            }
            else
            {
                MessageBox.Show("V dokumentu není vybraná povrchová úprava. Vyberte povrchovou úpravu ručně.", "EOkno", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.VybranaPU = this.PovrchoveUpravy[0];
            }

            for (int i = 0; i < this.Komponenty.Count; i++)
            {
                this.Komponenty[i].Vybrano = docVM.Komponenty[i].Vybrano;
            }
        }

        private bool _inheritFromDocument;
        public bool InheritFromDocument
        {
            get { return _inheritFromDocument; }
            set
            {
                if (_inheritFromDocument != value)
                {
                    _inheritFromDocument = value;
                    OnPropertyChanged(nameof(InheritFromDocument));

                    if (_data != null)
                    {
                        var attr = _data.Attribute(s_Inherit);
                        if (attr != null)
                        {
                            string properValue = (_inheritFromDocument) ? s_True : s_False;
                            if (attr.Value == properValue)
                            {
                                return;
                            }
                        }

                        _data.SetAttributeValue(s_Inherit, (_inheritFromDocument) ? s_True : s_False);
                        if (_inheritFromDocument)
                        {
                            // odstranit všechen obsah
                            _removedContent = _data.Elements().ToList();
                            _data.RemoveNodes();
                        }
                        else
                        {
                            // nechat zaregistrovat obsah
                            if (_removedContent != null && _removedContent.Count != 0)
                            {
                                // konfigurace podle uloženého nastavení
                                _data.Add(_removedContent);
                                base.SetMainElement(_data, false);
                            }
                            else
                            {
                                // výchozí stav podle volání z View
                                base.SetMainElement(_data, _created);
                            }
                        }
                    }
                }
            }
        }

        internal override void SetMainElement(XElement data, bool created)
        {
            _data = data;
            _created = created;

            if (created)
            {
                this.InheritFromDocument = true;
            }
            else
            {
                this.InheritFromDocument = data.GetAttrValue(s_Inherit, s_True) == s_True;
                if (!this.InheritFromDocument)
                {
                    base.SetMainElement(data, created);
                }
            }
        }
    }
}
