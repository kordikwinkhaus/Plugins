using System.Xml.Linq;
using Janosik.Models;
using Okna.Plugins.ViewModels;

namespace Janosik.ViewModels
{
    public class GlasspacketViewModel : ViewModelBase
    {
        private GlasspacketModel _model;

        public GlasspacketViewModel()
        {
            _model = new GlasspacketModel(new XElement(Xml.Presah));
        }

        internal void SetModel(GlasspacketModel model)
        {
            _model = model;
            OnPropertyChanged(nameof(PresahVlevo));
            OnPropertyChanged(nameof(PresahVpravo));
            OnPropertyChanged(nameof(PresahNahore));
            OnPropertyChanged(nameof(PresahDole));
        }

        public int PresahVlevo
        {
            get { return _model.PresahVlevo; }
            set
            {
                if (_model.PresahVlevo != value)
                {
                    _model.PresahVlevo = value;
                    OnPropertyChanged(nameof(PresahVlevo));
                }
            }
        }

        public int PresahVpravo
        {
            get { return _model.PresahVpravo; }
            set
            {
                if (_model.PresahVpravo != value)
                {
                    _model.PresahVpravo = value;
                    OnPropertyChanged(nameof(PresahVpravo));
                }
            }
        }

        public int PresahNahore
        {
            get { return _model.PresahNahore; }
            set
            {
                if (_model.PresahNahore != value)
                {
                    _model.PresahNahore = value;
                    OnPropertyChanged(nameof(PresahNahore));
                }
            }
        }

        public int PresahDole
        {
            get { return _model.PresahDole; }
            set
            {
                if (_model.PresahDole != value)
                {
                    _model.PresahDole = value;
                    OnPropertyChanged(nameof(PresahDole));
                }
            }
        }
    }
}
