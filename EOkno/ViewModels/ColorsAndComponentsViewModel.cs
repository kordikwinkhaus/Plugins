using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    public abstract class ColorsAndComponentsViewModel : ViewModelBase
    {
        internal XElement _data;

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
                    if (_data != null)
                    {
                        _vybranaPU.ZapsatOdstiny();
                    }
                }
            }
        }

        internal virtual void SetMainElement(XElement data, bool created)
        {
            _data = data;

            Clear();

            if (created)
            {
                ResetToDefault();
            }
            else
            {
                Init();
            }
        }
        /// <summary>
        /// Odstraní veškeré reference na XElementy, aby nedošlo k nechtěné úpravě
        /// XML z předchozího zobrazení.
        /// </summary>
        protected virtual void Clear()
        {
            this.PovrchoveUpravy.ForEach(p => p.Clear());
            this.Komponenty.ForEach(k => k.Clear());
        }

        /// <summary>
        /// Nastaví výchozí volby.
        /// </summary>
        protected virtual void ResetToDefault()
        {
            this.PovrchoveUpravy.ForEach(p => p.ResetToDefault(_data));
            this.VybranaPU = this.PovrchoveUpravy.First();

            this.Komponenty.ForEach(k => k.ResetToDefault(_data));
        }

        internal virtual void NotifyChange()
        {
        }

        /// <summary>
        /// Nastaví volby podle uloženého XML.
        /// </summary>
        protected virtual void Init()
        {
            foreach (var povrchUprava in this.PovrchoveUpravy)
            {
                if (povrchUprava.Init(_data))
                {
                    this.VybranaPU = povrchUprava;
                }
            }

            this.Komponenty.ForEach(k => k.Init(_data));
        }
    }
}
