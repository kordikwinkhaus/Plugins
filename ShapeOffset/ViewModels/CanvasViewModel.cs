using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Okna.Plugins;
using Okna.Plugins.ViewModels;

namespace ShapeOffset.ViewModels
{
    public class CanvasViewModel : ViewModelBase
    {
        LineViewModel _lastLine;
        LineViewModel _closeLine;
        List<Point> _points = new List<Point>();

        public CanvasViewModel()
        {
            this.CloseShapeCommand = new RelayCommand(CloseShape);
        }

        public ObservableCollection<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

        private Point _mousePosition;
        public Point MousePosition
        {
            get { return _mousePosition; }
            set
            {
                _mousePosition = value;

                if (_lastLine != null)
                {
                    _lastLine.X2 = _mousePosition.X;
                    _lastLine.Y2 = _mousePosition.Y;
                }
                if (_closeLine != null)
                {
                    _closeLine.X1 = _mousePosition.X;
                    _closeLine.Y1 = _mousePosition.Y;
                }
            }
        }

        private bool _closedShape;
        public bool ClosedShape
        {
            get { return _closedShape; }
            set
            {
                if (_closedShape != value)
                {
                    _closedShape = value;
                    OnPropertyChanged(nameof(ClosedShape));
                }
            }
        }

        public ICommand CloseShapeCommand { get; private set; }

        private void CloseShape(object param)
        {
            if (ClosedShape) return;
            if (_points.Count < 3) return;

            this.Items.Remove(_lastLine);
            _lastLine = null;
            this.Items.Remove(_closeLine);
            _closeLine = null;

            var firstPoint = _points.First();
            var lastPoint = _points.Last();
            var line = new LineViewModel(lastPoint.X, lastPoint.Y, firstPoint.X, firstPoint.Y);
            this.Items.Add(line);
            this.ClosedShape = true;
        }

        internal bool NotifyMouseClick(Point currentPoint)
        {
            if (ClosedShape) return false;

            currentPoint = Snap.ToGrid(currentPoint);

            if (_points.Count != 0)
            {
                // check if new point has same coordinations as last point
                var lastPoint = _points.Last();
                if (DoubleUtils.Equals(lastPoint, currentPoint)) return true;

                // check if new point has same coordinations as first pont
                var firstPoint = _points[0];
                if (DoubleUtils.Equals(firstPoint, currentPoint))
                {
                    CloseShape();
                    return true;
                }
            }

            _points.Add(currentPoint);
            _lastLine = new LineViewModel(currentPoint.X, currentPoint.Y, _mousePosition.X, _mousePosition.Y);
            this.Items.Add(_lastLine);

            if (_closeLine == null && this.Items.Count == 2)
            {
                var firstPoint = _points[0];
                _closeLine = new LineViewModel(_mousePosition.X, _mousePosition.Y, firstPoint.X, firstPoint.Y);
                this.Items.Add(_closeLine);
            }
            
            return true;
        }

        private void CloseShape()
        {
            var firstPoint = _points[0];

            _lastLine.X2 = firstPoint.X;
            _lastLine.Y2 = firstPoint.Y;
            this.Items.Remove(_closeLine);
            _closeLine = null;
            _lastLine = null;

            ClosedShape = true;
        }
    }
}
