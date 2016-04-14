using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using EOkno.Models;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    public abstract class ColorsAndComponentsViewModel : ViewModelBase
    {
        private ColorsAndComponents _model;

        internal ColorsAndComponentsViewModel()
        {
            this.SelectAllCommand = new RelayCommand(SelectAll);
            this.DeselectAllCommand = new RelayCommand(DeselectAll);
        }

        public ICommand SelectAllCommand { get; private set; }

        private void SelectAll(object param)
        {
            this.Komponenty.ForEach(k => k.Vybrano = true);
        }

        public ICommand DeselectAllCommand { get; private set; }

        private void DeselectAll(object param)
        {
            this.Komponenty.ForEach(k => k.Vybrano = false);
        }

        public List<KomponentaViewModel> Komponenty { get; private set; } = new List<KomponentaViewModel>();

        public List<PovrchovaUpravaViewModel> PovrchoveUpravy { get; set; } = new List<PovrchovaUpravaViewModel>();

        private PovrchovaUpravaViewModel _vybranaPU;
        public PovrchovaUpravaViewModel VybranaPU
        {
            get { return _vybranaPU; }
            set
            {
                if (_vybranaPU != value)
                {
                    _vybranaPU = value;
                    OnPropertyChanged(nameof(VybranaPU));

                    _model.PovrchovaUpravaKod = _vybranaPU.Kod;
                    _vybranaPU.ZapsatOdstiny();
                }
            }
        }

        internal void SetModel(ColorsAndComponents model)
        {
            _model = model;

            Init();
        }

        private void Init()
        {
            foreach (var povrchUprava in this.PovrchoveUpravy)
            {
                if (povrchUprava.Init(_model))
                {
                    this.VybranaPU = povrchUprava;
                }
            }

            this.Komponenty.ForEach(k => k.Init(_model));
        }

        internal virtual void SetDefaults()
        {
            this.VybranaPU = this.PovrchoveUpravy.FirstOrDefault();
            this.VybranaPU?.Clear();

            this.Komponenty.ForEach(k => k.Vybrano = true);
        }

        internal virtual void NotifyChange()
        {
        }
    }
}
