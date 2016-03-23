using System;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    public class KomponentaViewModel : ViewModelBase
    {
        internal KomponentaViewModel(string nazev, string material, string prace)
        {
            if (string.IsNullOrEmpty(nazev)) throw new ArgumentNullException(nameof(nazev));
            if (string.IsNullOrEmpty(material) && string.IsNullOrEmpty(prace))
            {
                throw new ArgumentException("Je třeba zadat materiál nebo práci.");
            }

            this.Nazev = nazev;
            this.Material = material;
            this.Prace = prace;
        }

        public string Nazev { get; private set; }
        public string Material { get; private set; }
        public string Prace { get; private set; }

        private bool _vybrano;
        public bool Vybrano
        {
            get { return _vybrano; }
            set
            {
                if (_vybrano != value)
                {
                    _vybrano = value;
                    OnPropertyChanged(nameof(Vybrano));
                }
            }
        }
    }
}
