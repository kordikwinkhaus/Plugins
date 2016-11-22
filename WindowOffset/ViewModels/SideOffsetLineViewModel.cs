using Okna.Plugins.ViewModels;
using WindowOffset.Models;

namespace WindowOffset.ViewModels
{
    public class SideOffsetLineViewModel : ViewModelBase, IScaleable
    {
        private readonly SideOffset _model;

        internal SideOffsetLineViewModel(SideOffset model)
        {
            _model = model;
        }

        public void Recalculate(double scale, double left, double top, double dimLayerHeight)
        {
            this.X1 = _model.Start.X / scale + left;
            this.Y1 = _model.Start.Y / scale + top;
            this.X2 = _model.End.X / scale + left;
            this.Y2 = _model.End.Y / scale + top;
        }

        private double _x1;
        public double X1
        {
            get { return _x1; }
            set
            {
                if (_x1 != value)
                {
                    _x1 = value;
                    OnPropertyChanged(nameof(X1));
                }
            }
        }

        private double _y1;
        public double Y1
        {
            get { return _y1; }
            set
            {
                if (_y1 != value)
                {
                    _y1 = value;
                    OnPropertyChanged(nameof(Y1));
                }
            }
        }

        private double _x2;
        public double X2
        {
            get { return _x2; }
            set
            {
                if (_x2 != value)
                {
                    _x2 = value;
                    OnPropertyChanged(nameof(X2));
                }
            }
        }

        private double _y2;
        public double Y2
        {
            get { return _y2; }
            set
            {
                if (_y2 != value)
                {
                    _y2 = value;
                    OnPropertyChanged(nameof(Y2));
                }
            }
        }
    }
}
