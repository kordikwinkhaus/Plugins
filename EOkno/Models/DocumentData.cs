using System.Globalization;
using System.Xml.Linq;

namespace EOkno.Models
{
    internal class DocumentData : ColorsAndComponents
    {
        /// <param name="data">XML element pluginu EOkno pro dokument.</param>
        public DocumentData(XElement data)
            : base(data)
        {
            _sleva = GetDecimal(Xml.Sleva);
            _dph = GetDecimal(Xml.Dph);
        }

        private decimal GetDecimal(string attrName)
        {
            XAttribute attr = _data.Attribute(attrName);
            if (attr != null)
            {
                decimal attrAsNumber;
                if (decimal.TryParse(attr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out attrAsNumber))
                {
                    return attrAsNumber;
                }
            }

            return 0;
        }

        private decimal _sleva;
        internal decimal Sleva
        {
            get { return _sleva; }
            set
            {
                _sleva = value;
                _data.SetAttributeValue(Xml.Sleva, _sleva.ToString(CultureInfo.InvariantCulture));
            }
        }

        private decimal _dph;
        public decimal DPH
        {
            get { return _dph; }
            set
            {
                _dph = value;
                _data.SetAttributeValue(Xml.Dph, _dph.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
