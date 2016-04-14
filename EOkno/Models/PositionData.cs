using System.Xml.Linq;
using Okna.Plugins;

namespace EOkno.Models
{
    internal class PositionData : ColorsAndComponents
    {
        /// <param name="data">XML element pluginu EOkno pro pozici.</param>
        public PositionData(XElement data)
            : base(data)
        {
            _podleDokumentu = data.GetAttrValue(Xml.Inherit, Xml.True) == Xml.True;
        }

        private bool _podleDokumentu;
        internal bool PodleDokumentu
        {
            get { return _podleDokumentu; }
            set
            {
                if (_podleDokumentu != value)
                {
                    _podleDokumentu = value;

                    _data.SetAttributeValue(Xml.Inherit, (value) ? Xml.True : Xml.False);
                }
            }
        }
    }
}
