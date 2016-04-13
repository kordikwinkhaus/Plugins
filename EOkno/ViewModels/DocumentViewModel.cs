using Okna.Plugins.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;

namespace EOkno.ViewModels
{
    public class DocumentViewModel : ViewModelBase
    {
        private const string s_sleva = "s";
        private XElement _data;

        public DocumentViewModel()
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

        private decimal _sleva;
        public decimal Sleva
        {
            get { return _sleva; }
            set
            {
                if (_sleva != value)
                {
                    _sleva = value;
                    OnPropertyChanged(nameof(Sleva));
                    if (_data != null)
                    {
                        UlozitSlevuDoXml();
                    }
                }
            }
        }

        private void UlozitSlevuDoXml()
        {
            var slevaAttr = _data.Attribute(s_sleva);
            if (slevaAttr == null)
            {
                slevaAttr = new XAttribute(s_sleva, string.Empty);
                _data.Add(slevaAttr);
            }
            slevaAttr.Value = _sleva.ToString(CultureInfo.InvariantCulture);
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
        private void Clear()
        {
            this.PovrchoveUpravy.ForEach(p => p.Clear());
            this.Komponenty.ForEach(k => k.Clear());
        }

        /// <summary>
        /// Nastaví výchozí volby.
        /// </summary>
        private void ResetToDefault()
        {
            this.Sleva = 0;
            this.PovrchoveUpravy.ForEach(p => p.ResetToDefault(_data));
            this.VybranaPU = this.PovrchoveUpravy.First();

            this.Komponenty.ForEach(k => k.ResetToDefault(_data));
        }

        /// <summary>
        /// Nastaví volby podle uloženého XML.
        /// </summary>
        private void Init()
        {
            foreach (var povrchUprava in this.PovrchoveUpravy)
            {
                if (povrchUprava.Init(_data))
                {
                    this.VybranaPU = povrchUprava;
                }
            }

            this.Komponenty.ForEach(k => k.Init(_data));

            XAttribute slevaAttr = _data.Attribute(s_sleva);
            if (slevaAttr != null)
            {
                decimal sleva;
                if (decimal.TryParse(slevaAttr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out sleva))
                {
                    this.Sleva = sleva;
                }
            }
        }
    }
}
