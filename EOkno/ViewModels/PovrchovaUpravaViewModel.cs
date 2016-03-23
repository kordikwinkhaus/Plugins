using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    public class PovrchovaUpravaViewModel : ViewModelBase
    {
        private const string s_PovrchUprava = "p";
        private const string s_UpravaKod = "k";
        private const string s_OdstinExterier = "e";
        private const string s_OdstinInterier = "i";

        private XElement _data;
        private XElement _povrchUprava;

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
                    SetVnitrniOdstin();
                }
            }
        }

        private void SetVnitrniOdstin()
        {
            if (_povrchUprava != null)
            {
                string kod = _vnitrniOdstin?.Kod;
                _povrchUprava.SetAttributeValue(s_OdstinInterier, kod);
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
            if (_povrchUprava != null)
            {
                string kod = _vnejsiOdstin?.Kod;
                _povrchUprava.SetAttributeValue(s_OdstinExterier, kod);
            }
        }

        /// <summary>
        /// Odstraní veškeré reference na XElementy, aby nedošlo k nechtěné úpravě
        /// XML z předchozího zobrazení.
        /// </summary>
        internal void Clear()
        {
            _data = null;
            _povrchUprava = null;

            this.VnejsiOdstin = null;
            this.VnitrniOdstin = null;
        }

        /// <summary>
        /// Nastaví výchozí volby a zaregistruje si hlavní element pluginu.
        /// </summary>
        /// <param name="data">Hlavní element pluginu.</param>
        internal void ResetToDefault(XElement data)
        {
            this.VnejsiOdstin = null;
            this.VnitrniOdstin = null;

            _data = data;
        }

        /// <summary>
        /// Nastaví volby podle uloženého XML.
        /// </summary>
        /// <param name="data">Hlavní element pluginu.</param>
        internal bool Init(XElement data)
        {
            _data = data;

            var element = data.Element(s_PovrchUprava);
            if (element != null)
            {
                var attr = element.Attribute(s_UpravaKod);
                if (attr != null)
                {
                    if (attr.Value == this.Kod)
                    {
                        if (this.MaOdstiny)
                        {
                            InitOdstiny(element);
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        private void InitOdstiny(XElement elem)
        {
            var attrExt = elem.Attribute(s_OdstinExterier);
            if (attrExt != null)
            {
                this.VnejsiOdstin = this.Odstiny.FirstOrDefault(o => o.Kod == attrExt.Value);
            }

            var attrInt = elem.Attribute(s_OdstinInterier);
            if (attrInt != null)
            {
                this.VnitrniOdstin = this.Odstiny.FirstOrDefault(o => o.Kod == attrInt.Value);
            }
        }

        /// <summary>
        /// Volá se po vybrání dané povrchové úpravy. 
        /// Účelem je zápis dat dané povrchové úpravy do elementu povrchové úpravy.
        /// </summary>
        internal void ZapsatOdstiny()
        {
            _povrchUprava = _data.Element(s_PovrchUprava);
            if (_povrchUprava == null)
            {
                _povrchUprava = new XElement(s_PovrchUprava);
                _data.Add(_povrchUprava);
            }
            _povrchUprava.SetAttributeValue(s_UpravaKod, this.Kod);

            SetVnejsiOdstin();
            SetVnitrniOdstin();
        }
    }
}
