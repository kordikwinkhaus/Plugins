using System;
using System.Collections.Generic;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    public class PovrchovaUpravaViewModel : ViewModelBase
    {
        internal PovrchovaUpravaViewModel(string kod, string nazev)
        {
            if (string.IsNullOrEmpty(kod)) throw new ArgumentNullException(nameof(kod));
            if (string.IsNullOrEmpty(nazev)) throw new ArgumentNullException(nameof(nazev));

            this.Kod = kod;
            this.Nazev = nazev;
        }

        public string Kod { get; private set; }
        public string Nazev { get; private set; }

        public bool MaOdstiny
        {
            get { return this.Odstiny.Count != 0; }
        }

        public List<OdstinViewModel> Odstiny { get;  set; }

        private OdstinViewModel _vnitrniOdstin;
        public OdstinViewModel VnitrniOdstin
        {
            get { return _vnitrniOdstin; }
            set
            {
                if (_vnitrniOdstin != value)
                {
                    _vnitrniOdstin = value;
                    OnPropertyChanged(nameof(VnitrniOdstin));
                }
            }
        }

        private OdstinViewModel _vnejsiOdstin;
        public OdstinViewModel VnejsiOdstin
        {
            get { return _vnejsiOdstin; }
            set
            {
                if (_vnejsiOdstin != value)
                {
                    _vnejsiOdstin = value;
                    OnPropertyChanged(nameof(VnejsiOdstin));
                }
            }
        }
    }
}
