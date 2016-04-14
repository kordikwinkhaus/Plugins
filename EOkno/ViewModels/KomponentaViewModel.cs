using System;
using System.Diagnostics;
using EOkno.Models;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    [DebuggerDisplay("{Nazev} (vybrano:{Vybrano})")]
    public class KomponentaViewModel : ViewModelBase
    {
        private readonly Func<ColorsAndComponents, bool> _jeVybrano;
        private readonly Action<ColorsAndComponents, bool> _zmenitVybrano;
        private ColorsAndComponents _model;

        private readonly ColorsAndComponentsViewModel _parent;

        internal KomponentaViewModel(string nazev, string material, string prace, ColorsAndComponentsViewModel parent)
        {
            if (string.IsNullOrEmpty(nazev)) throw new ArgumentNullException(nameof(nazev));
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            if (!string.IsNullOrEmpty(material))
            {
                string kod = material;
                _jeVybrano = m => m.JeVybranMaterial(kod);
                _zmenitVybrano = (m, b) => m.ZmenitVyberMaterialu(kod, b);
            }
            else if (!string.IsNullOrEmpty(prace))
            {
                string kod = prace;
                _jeVybrano = m => m.JeVybranaPrace(kod);
                _zmenitVybrano = (m, b) => m.ZmenitVyberPrace(kod, b);
            }
            else
            {
                throw new ArgumentException("Je třeba zadat materiál nebo práci.");
            }

            this.Nazev = nazev;
            this.Material = material;
            this.Prace = prace;
            _parent = parent;
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
                    OnPropertyChanged(nameof(VybranoRozdil));

                    if (_model != null)
                    {
                        _zmenitVybrano(_model, _vybrano);
                    }
                }
            }
        }

        private bool _vybranoDokument;
        public bool VybranoDokument
        {
            get { return _vybranoDokument; }
            set
            {
                if (_vybranoDokument != value)
                {
                    _vybranoDokument = value;
                    OnPropertyChanged(nameof(VybranoDokument));
                    OnPropertyChanged(nameof(VybranoRozdil));
                }
            }
        }

        public bool VybranoRozdil
        {
            get { return this.Vybrano != this.VybranoDokument; }
        }

        internal void Init(ColorsAndComponents model)
        {
            _model = model;
            this.Vybrano = _jeVybrano(_model);
        }
    }
}
