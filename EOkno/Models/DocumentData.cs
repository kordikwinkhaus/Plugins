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
            XAttribute slevaAttr = _data.Attribute(Xml.Sleva);
            if (slevaAttr != null)
            {
                decimal sleva;
                if (decimal.TryParse(slevaAttr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out sleva))
                {
                    _sleva = sleva;
                }
            }
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
    }
}
