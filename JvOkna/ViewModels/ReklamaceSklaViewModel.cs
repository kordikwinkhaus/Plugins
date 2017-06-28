using System.Xml.Linq;
using Okna.Plugins.ViewModels;

namespace JvOkna.ViewModels
{
    public class ReklamaceSklaViewModel : ViewModelBase
    {
        private XElement _pluginElement;

        public ReklamaceSklaViewModel()
        {
            _pocet = 1;
        }

        internal void Init(XElement pluginElement)
        {
            _pluginElement = pluginElement;
            var attr = pluginElement.Attribute(Xml.Reklamovat);
            if (attr != null)
            {
                this.ReklamovatSklo = attr.Value == Xml.On;
            }
            int val;
            if (int.TryParse(_pluginElement.Value, out val))
            {
                this.Pocet = val;
            }
        }

        private void Sync()
        {
            var attr = _pluginElement.Attribute(Xml.Reklamovat);
            if (attr != null)
            {
                attr.Value = (_reklamovatSklo) ? Xml.On : Xml.Off;
            }
            else if (_reklamovatSklo)
            {
                _pluginElement.SetAttributeValue(Xml.Reklamovat, Xml.On);
            }

            _pluginElement.Value = _pocet.ToString();
        }

        private bool _reklamovatSklo;
        public bool ReklamovatSklo
        {
            get { return _reklamovatSklo; }
            set
            {
                if (_reklamovatSklo != value)
                {
                    _reklamovatSklo = value;
                    Sync();
                    OnPropertyChanged(nameof(ReklamovatSklo));
                }
            }
        }

        private int _pocet;
        public int Pocet
        {
            get { return _pocet; }
            set
            {
                if (_pocet != value)
                {
                    _pocet = value;
                    Sync();
                    OnPropertyChanged(nameof(Pocet));
                }
            }
        }
    }
}
