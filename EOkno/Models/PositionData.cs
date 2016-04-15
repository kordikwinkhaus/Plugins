using System;
using System.Xml.Linq;

namespace EOkno.Models
{
    internal class PositionData : ColorsAndComponents
    {
        /// <param name="data">XML element pluginu EOkno pro pozici.</param>
        public PositionData(XElement data)
            : base(data)
        {
            var attr = data.Attribute(Xml.Inherit);
            if (attr != null)
            {
                _podleDokumentu = string.Compare(Xml.True, attr.Value, StringComparison.InvariantCulture) == 0;
            }
            else
            {
                _data.SetAttributeValue(Xml.Inherit, Xml.True);
                _podleDokumentu = true;
            }
        }

        private bool _podleDokumentu;
        internal bool PodleDokumentu
        {
            get { return _podleDokumentu; }
            set
            {
                _podleDokumentu = value;
                _data.SetAttributeValue(Xml.Inherit, (value) ? Xml.True : Xml.False);
            }
        }

        internal bool IsEmpty()
        {
            if (this.PodleDokumentu) return false;
            if (_material.Count != 0) return false;
            if (_prace.Count != 0) return false;
            if (!string.IsNullOrEmpty(this.PovrchovaUpravaKod)) return false;

            return true;
        }
    }
}
