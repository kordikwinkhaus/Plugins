using System.Xml.Linq;

namespace Janosik.Models
{
    internal class GlasspacketModel
    {
        private readonly XElement _data;

        internal GlasspacketModel(XElement data)
        {
            _data = data;

            _presahDole = GetPresah(data, Xml.PresahDole);
            _presahNahore = GetPresah(data, Xml.PresahNahore);
            _presahVlevo = GetPresah(data, Xml.PresahVlevo);
            _presahVpravo = GetPresah(data, Xml.PresahVpravo);

            SetMaPresah();
        }

        private int GetPresah(XElement data, string attrName)
        {
            var attr = data.Attribute(attrName);
            if (attr != null)
            {
                int presah;
                if (int.TryParse(attr.Value, out presah))
                {
                    return presah;
                }
            }

            return 0;
        }

        private int _presahDole;
        internal int PresahDole
        {
            get { return _presahDole; }
            set
            {
                _presahDole = value;
                _data.SetAttributeValue(Xml.PresahDole, value);
                SetMaPresah();
            }
        }

        private int _presahNahore;
        internal int PresahNahore
        {
            get { return _presahNahore; }
            set
            {
                _presahNahore = value;
                _data.SetAttributeValue(Xml.PresahNahore, value);
                SetMaPresah();
            }
        }

        private int _presahVlevo;
        internal int PresahVlevo
        {
            get { return _presahVlevo; }
            set
            {
                _presahVlevo = value;
                _data.SetAttributeValue(Xml.PresahVlevo, value);
                SetMaPresah();
            }
        }

        private int _presahVpravo;
        internal int PresahVpravo
        {
            get { return _presahVpravo; }
            set
            {
                _presahVpravo = value;
                _data.SetAttributeValue(Xml.PresahVpravo, value);
                SetMaPresah();
            }
        }

        private void SetMaPresah()
        {
            bool maPresah = _presahDole != 0
                || _presahNahore != 0
                || _presahVlevo != 0
                || _presahVpravo != 0;

            _data.SetAttributeValue(Xml.MaPresah, (maPresah) ? Xml.True : Xml.False);
        }
    }
}
