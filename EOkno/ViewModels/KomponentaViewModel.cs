using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Okna.Plugins;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    public class KomponentaViewModel : ViewModelBase
    {
        private const string s_komponenta = "k";
        private const string s_prace = "p";
        private const string s_material = "m";

        private XElement _data;

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

                    if (_data != null)
                    {
                        if (_vybrano)
                        {
                            // přidat element, pokud neexistuje
                            XElement el;
                            if (!ExistsElement(out el))
                            {
                                _data.Add(el);
                            }
                        }
                        else
                        {
                            // odebrat element, pokud existuje
                            XElement el;
                            if (ExistsElement(out el))
                            {
                                el.Remove();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Odstraní veškeré reference na XElementy, aby nedošlo k nechtěné úpravě
        /// XML z předchozího zobrazení.
        /// </summary>
        internal void Clear()
        {
            _data = null;
            this.Vybrano = false;
        }

        /// <summary>
        /// Nastaví výchozí volby a zaregistruje si hlavní element pluginu.
        /// </summary>
        /// <param name="data">Hlavní element pluginu.</param>
        internal void ResetToDefault(XElement data)
        {
            _data = data;
            this.Vybrano = true;
        }

        /// <summary>
        /// Nastaví volby podle uloženého XML.
        /// </summary>
        /// <param name="data">Hlavní element pluginu.</param>
        internal void Init(XElement data)
        {
            _data = data;

            XElement el;
            this.Vybrano = ExistsElement(out el);
        }

        private bool ExistsElement(out XElement elem)
        {
            string attrName = s_material;
            string attrVal = this.Material;
            if (!string.IsNullOrEmpty(this.Prace))
            {
                attrName = s_prace;
                attrVal = this.Prace;
            }
            string n = attrName;
            string v = attrVal;

            elem = _data.Elements(s_komponenta).FirstOrDefault(k => k.HasAttribute(n, v));
            if (elem == null)
            {
                elem = new XElement(s_komponenta, new XAttribute(attrName, attrVal));
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
