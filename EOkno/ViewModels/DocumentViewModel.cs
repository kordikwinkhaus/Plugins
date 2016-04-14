using System.Globalization;
using System.Xml.Linq;

namespace EOkno.ViewModels
{
    public class DocumentViewModel : ColorsAndComponentsViewModel
    {
        private const string s_sleva = "s";

        internal DocumentViewModel()
        {
        }

        internal MessageBroker Broker { get; set; }

        internal override void NotifyChange()
        {
            if (this.Broker != null)
            {
                this.Broker.DocumentUpdated(this);
            }
        }

        private decimal _sleva;
        public decimal Sleva
        {
            get { return _sleva; }
            set
            {
                if (_sleva != value)
                {
                    _sleva = value;
                    OnPropertyChanged(nameof(Sleva));
                    if (_data != null)
                    {
                        UlozitSlevuDoXml();
                    }
                }
            }
        }

        private void UlozitSlevuDoXml()
        {
            var slevaAttr = _data.Attribute(s_sleva);
            if (slevaAttr == null)
            {
                slevaAttr = new XAttribute(s_sleva, string.Empty);
                _data.Add(slevaAttr);
            }
            slevaAttr.Value = _sleva.ToString(CultureInfo.InvariantCulture);
        }

        protected override void ResetToDefault()
        {
            base.ResetToDefault();

            this.Sleva = 0;
        }

        protected override void Init()
        {
            base.Init();

            XAttribute slevaAttr = _data.Attribute(s_sleva);
            if (slevaAttr != null)
            {
                decimal sleva;
                if (decimal.TryParse(slevaAttr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out sleva))
                {
                    this.Sleva = sleva;
                }
            }
        }
    }
}
