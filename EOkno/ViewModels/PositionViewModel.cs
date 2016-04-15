using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using EOkno.Models;
using UserExt;
using WHOkna;

namespace EOkno.ViewModels
{
    public class PositionViewModel : ColorsAndComponentsViewModel
    {
        private PositionData _model;

        internal PositionViewModel()
        {
            this.CopyFromDocumentCommand = new RelayCommand(CopyFromDocument);
            this.CopyColorsCommand = new RelayCommand(CopyColors);
            this.CopyComponentsCommand = new RelayCommand(CopyComponents);
        }

        internal void SetModel(PositionData model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            base.SetModel(model);
            _model = model;

            this.InheritFromDocument = _model.PodleDokumentu;
        }

        private void model_DataChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        internal IOknaDocument OknaDocument { get; set; }

        private DocumentData GetDocumentData()
        {
            try
            {
                XElement data = this.OknaDocument.ExtendedProperties;
                XElement eokno = data.Element(Xml.EOkno);
                if (eokno != null)
                {
                    var eoknoClone = new XElement(eokno);
                    return new DocumentData(eoknoClone);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public ICommand CopyFromDocumentCommand { get; private set; }

        private void CopyFromDocument(object param)
        {
            CopyFromDocument(copyColors: true, copyComponents: true);
        }

        public ICommand CopyColorsCommand { get; private set; }

        private void CopyColors(object param)
        {
            CopyFromDocument(copyColors: true, copyComponents: false);
        }

        public ICommand CopyComponentsCommand { get; private set; }

        private void CopyComponents(object param)
        {
            CopyFromDocument(copyColors: false, copyComponents: true);
        }

        private void CopyFromDocument(bool copyColors, bool copyComponents, bool copyComponentFlagsOnly = false)
        {
            var docData = GetDocumentData();
            if (docData == null) return;

            DocumentViewModel docVM = new DocumentViewModel();
            ExtensionsFactory.Nacist(docVM);
            docVM.SetModel(docData);

            CopyFromDocumentCore(copyColors, copyComponents, copyComponentFlagsOnly, docVM);
        }

        private void CopyFromDocumentCore(bool copyColors, bool copyComponents, bool copyComponentFlagsOnly, DocumentViewModel document)
        {
            if (copyColors)
            {
                if (document.VybranaPU != null)
                {
                    this.VybranaPU = this.PovrchoveUpravy.SingleOrDefault(p => p.Kod == document.VybranaPU.Kod);
                    if (this.VybranaPU != null)
                    {
                        this.VybranaPU.VnejsiOdstin = this.VybranaPU.Odstiny.SingleOrDefault(o => o.Kod == document.VybranaPU.VnejsiOdstin?.Kod);
                        this.VybranaPU.VnitrniOdstin = this.VybranaPU.Odstiny.SingleOrDefault(o => o.Kod == document.VybranaPU.VnitrniOdstin?.Kod);
                        if (!this.InheritFromDocument)
                        {
                            this.VybranaPU.ZapsatOdstiny();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("V dokumentu není vybraná povrchová úprava. Vyberte povrchovou úpravu ručně.", "EOkno", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    this.VybranaPU = this.PovrchoveUpravy[0];
                }
            }

            if (copyComponents)
            {
                for (int i = 0; i < this.Komponenty.Count; i++)
                {
                    this.Komponenty[i].VybranoDokument = this.Komponenty[i].Vybrano = document.Komponenty[i].Vybrano;
                }
            }
            else if (copyComponentFlagsOnly)
            {
                for (int i = 0; i < this.Komponenty.Count; i++)
                {
                    this.Komponenty[i].VybranoDokument = document.Komponenty[i].Vybrano;
                }
            }
        }

        private bool _inheritFromDocument;
        public bool InheritFromDocument
        {
            get { return _inheritFromDocument; }
            set
            {
                _inheritFromDocument = value;
                _model.PodleDokumentu = value;
                OnPropertyChanged(nameof(InheritFromDocument));
                OnPropertyChanged(nameof(NotInheritFromDocument));

                if (_model.PodleDokumentu)
                {
                    base.SetModel(GetDocumentData());
                }
                else
                {
                    bool isEmpty = _model.IsEmpty();
                    base.SetModel(_model);
                    if (isEmpty)
                    {
                        CopyFromDocument(copyColors: true, copyComponents: true);
                    }
                    else
                    {
                        CopyFromDocument(copyColors: false, copyComponents: false, copyComponentFlagsOnly: true);
                    }
                }
            }
        }

        public bool NotInheritFromDocument
        {
            get { return !InheritFromDocument; }
        }

        internal override void SetDefaults()
        {
            this.InheritFromDocument = true;
        }

        internal void DocumentUpdated(DocumentViewModel document)
        {
            if (this.InheritFromDocument)
            {
                CopyFromDocumentCore(copyColors: true, copyComponents: true, copyComponentFlagsOnly: false, document: document);
            }
            else
            {
                CopyFromDocumentCore(copyColors: false, copyComponents: false, copyComponentFlagsOnly: true, document: document);
            }
        }
    }
}
