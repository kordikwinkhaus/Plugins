using System;
using Okna.Plugins.ViewModels;
using WindowOffset.Models;

namespace WindowOffset.ViewModels
{
    public class DimensionViewModel : ViewModelBase, IScaleable
    {
        private readonly Dimension _model;

        internal DimensionViewModel(Dimension model)
        {
            _model = model;
        }

        public float Value
        {
            get { return _model.Value; }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        public bool IsFirst
        {
            get { return _model.From == 0; }
        }

        internal int Layer { get; set; }

        internal float ModelLength { get; set; }

        public bool IsVertical { get; internal set; }

        public void Recalculate(double scale, double left, double top, double dimLayerHeight)
        {
            if (IsVertical)
            {
                double x = 0;
                if (this.ModelLength != 0)
                {
                    x = this.ModelLength / scale + left + EditOffsetViewModel.WIDTH_MARGIN;
                }
                this.X = x + this.Layer * dimLayerHeight;
                this.Y = _model.From / scale + top;
            }
            else
            {
                this.X = _model.From / scale + left;
                double y = 0;
                if (this.ModelLength != 0)
                {
                    y = this.ModelLength / scale + top + EditOffsetViewModel.HEIGHT_MARGIN;
                }
                this.Y = y + this.Layer * dimLayerHeight;
            }
            this.Width = _model.Value / scale;
            this.Height = dimLayerHeight;
        }
    }
}
