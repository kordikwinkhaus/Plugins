using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EOkno.Models;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    [DebuggerDisplay("{Nazev} ({Kod})")]
    public class PovrchovaUpravaViewModel : ViewModelBase
    {
        private ColorsAndComponents _model;

        internal PovrchovaUpravaViewModel(string kod, string nazev, bool isDefault)
        {
            if (string.IsNullOrEmpty(kod)) throw new ArgumentNullException(nameof(kod));
            if (string.IsNullOrEmpty(nazev)) throw new ArgumentNullException(nameof(nazev));

            this.Kod = kod;
            this.Nazev = nazev;
            this.IsDefault = isDefault;
            this.Odstiny = new List<OdstinViewModel>();
        }

        public string Kod { get; private set; }
        public string Nazev { get; private set; }
        internal bool IsDefault { get; private set; }

        public bool MaOdstiny
        {
            get { return this.Odstiny.Count != 0; }
        }

        public List<OdstinViewModel> Odstiny { get; set; }

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
                    SetVnitrniOdstin();
                }
            }
        }

        private void SetVnitrniOdstin()
        {
            if (_model != null)
            {
                string kod = _vnitrniOdstin?.Kod;
                string nazev = _vnitrniOdstin?.Nazev;

                _model.SetOdstinInterier(kod, nazev);
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
                    SetVnejsiOdstin();
                }
            }
        }

        private void SetVnejsiOdstin()
        {
            if (_model != null)
            {
                string kod = _vnejsiOdstin?.Kod;
                string nazev = _vnejsiOdstin?.Nazev;

                _model.SetOdstinExterier(kod, nazev);
            }
        }

        internal void Clear()
        {
            this.VnitrniOdstin = this.VnejsiOdstin = this.Odstiny.SingleOrDefault(o => o.IsDefault);
        }

        internal bool Init(ColorsAndComponents model)
        {
            _model = model;

            if (_model.PovrchovaUpravaKod == this.Kod)
            {
                if (this.MaOdstiny)
                {
                    InitOdstiny();
                }
                return true;
            }

            return false;
        }

        private void InitOdstiny()
        {
            this.VnejsiOdstin = this.Odstiny.FirstOrDefault(o => o.Kod == _model.OdstinExterierKod);
            this.VnitrniOdstin = this.Odstiny.FirstOrDefault(o => o.Kod == _model.OdstinInterierKod);
        }

        /// <summary>
        /// Volá se po vybrání dané povrchové úpravy. 
        /// Účelem je zápis dat dané povrchové úpravy do elementu povrchové úpravy.
        /// </summary>
        internal void ZapsatOdstiny()
        {
            SetVnejsiOdstin();
            SetVnitrniOdstin();
        }
    }
}
