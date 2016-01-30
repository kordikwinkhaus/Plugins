using Okna.Plugins;

namespace ShapeOffset.ViewModels
{
    public class LineViewModel : ItemViewModel
    {
        public LineViewModel(double x1, double y1, double x2, double y2)
        {
            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
        }

        private double _x1;
        public double X1
        {
            get { return _x1; }
            set
            {
                if (!DoubleUtils.Equals(_x1, value))
                {
                    _x1 = value;
                    OnPropertyChanged(nameof(X1));
                }
            }
        }

        private double _x2;
        public double X2
        {
            get { return _x2; }
            set
            {
                if (!DoubleUtils.Equals(_x2, value))
                {
                    _x2 = value;
                    OnPropertyChanged(nameof(X2));
                }
            }
        }

        private double _y1;
        public double Y1
        {
            get { return _y1; }
            set
            {
                if (!DoubleUtils.Equals(_y1, value))
                {
                    _y1 = value;
                    OnPropertyChanged(nameof(Y1));
                }
            }
        }

        private double _y2;
        public double Y2
        {
            get { return _y2; }
            set
            {
                if (!DoubleUtils.Equals(_y2, value))
                {
                    _y2 = value;
                    OnPropertyChanged(nameof(Y2));
                }
            }
        }
    }
}
