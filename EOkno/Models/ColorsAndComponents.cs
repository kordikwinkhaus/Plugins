using System.Collections.Generic;
using System.Xml.Linq;

namespace EOkno.Models
{
    internal class ColorsAndComponents
    {
        private readonly XElement _data;
        private readonly Komponenty _material;
        private readonly Komponenty _prace;
        private XElement _povrchUprava;

        /// <param name="data">XML element pluginu EOkno.</param>
        internal ColorsAndComponents(XElement data)
        {
            _data = data;
            _material = new Komponenty(Xml.Komponenta, Xml.Material, data);
            _prace = new Komponenty(Xml.Komponenta, Xml.Prace, data);

            InitPovrchovaUprava();
        }

        #region Povrchová úprava

        private void InitPovrchovaUprava()
        {
            _povrchUprava = _data.Element(Xml.PovrchUprava);
            if (_povrchUprava != null)
            {
                var attr = _povrchUprava.Attribute(Xml.UpravaKod);
                if (attr != null)
                {
                    _povrchovaUpravaKod = attr.Value;
                }

                attr = _povrchUprava.Attribute(Xml.OdstinExterier);
                if (attr != null)
                {
                    this.OdstinExterierKod = attr.Value;
                }
                attr = _povrchUprava.Attribute(Xml.OdstinInterier);
                if (attr != null)
                {
                    this.OdstinInterierKod = attr.Value;
                }
            }
            else
            {
                _povrchUprava = new XElement(Xml.PovrchUprava);
                _data.Add(_povrchUprava);
            }
        }

        private string _povrchovaUpravaKod;
        internal string PovrchovaUpravaKod
        {
            get { return _povrchovaUpravaKod; }
            set
            {
                if (_povrchovaUpravaKod != value)
                {
                    if (!string.IsNullOrEmpty(_povrchovaUpravaKod))
                    {
                        this.VymazatOdstiny();
                    }

                    _povrchovaUpravaKod = value;
                    _povrchUprava.SetAttributeValue(Xml.UpravaKod, value);
                }
            }
        }

        internal string OdstinExterierKod { get; private set; }

        internal string OdstinInterierKod { get; private set; }

        internal void SetOdstinExterier(string kod, string nazev)
        {
            this.OdstinExterierKod = kod;

            _povrchUprava.SetAttributeValue(Xml.OdstinExterier, kod);
            _povrchUprava.SetAttributeValue(Xml.OdstinNazevExterier, nazev);
        }

        internal void SetOdstinInterier(string kod, string nazev)
        {
            this.OdstinInterierKod = kod;

            _povrchUprava.SetAttributeValue(Xml.OdstinInterier, kod);
            _povrchUprava.SetAttributeValue(Xml.OdstinNazevInterier, nazev);
        }

        /// <summary>
        /// Používá se pro povrchové úpravy, které nemají odstíny.
        /// </summary>
        internal void VymazatOdstiny()
        {
            _povrchUprava.SetAttributeValue(Xml.OdstinExterier, null);
            _povrchUprava.SetAttributeValue(Xml.OdstinInterier, null);
            _povrchUprava.SetAttributeValue(Xml.OdstinNazevExterier, null);
            _povrchUprava.SetAttributeValue(Xml.OdstinNazevInterier, null);
        }

        #endregion

        #region Práce a komponenty

        internal bool JeVybranaKomponenta(string kod)
        {
            return _material.JeVybrana(kod);
        }

        internal void ZmenitVyberKomponenty(string kod, bool vybrano)
        {
            _material.ZmenitVyber(kod, vybrano);
        }

        internal bool VybranaPrace(string kod)
        {
            return _prace.JeVybrana(kod);
        }

        internal void ZmenitVyberPrace(string kod, bool vybrano)
        {
            _prace.ZmenitVyber(kod, vybrano);
        }

        #endregion

        #region Nested classes

        internal class Komponenty 
        {
            private readonly Dictionary<string, XElement> _komponenty;
            private readonly string _elemName;
            private readonly string _attrName;
            private readonly XElement _data;

            internal Komponenty(string elemName, string attrName, XElement data)
            {
                _komponenty = new Dictionary<string, XElement>();
                _elemName = elemName;
                _attrName = attrName;
                _data = data;

                foreach (XElement elem in data.Elements(_elemName))
                {
                    var attr = elem.Attribute(_attrName);
                    if (attr != null)
                    {
                        _komponenty.Add(attr.Value, elem);
                    }
                }
            }

            internal bool JeVybrana(string kod)
            {
                return _komponenty.ContainsKey(kod);
            }

            internal void ZmenitVyber(string kod, bool vybrano)
            {
                if (vybrano)
                {
                    if (!_komponenty.ContainsKey(kod))
                    {
                        var elem = new XElement(_elemName, new XAttribute(_attrName, kod));
                        _data.Add(elem);
                        _komponenty.Add(kod, elem);
                    }
                }
                else
                {
                    if (_komponenty.ContainsKey(kod))
                    {
                        var elem = _komponenty[kod];
                        elem.Remove();
                        _komponenty.Remove(kod);
                    }
                }
            }
        }

        #endregion
    }
}
