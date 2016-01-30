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
            this.ClearShapeCommand = new RelayCommand(ClearShape);
        }

        public ObservableCollection<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

        private ItemViewModel _selectedItem;
        public ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        #region Drawing shape

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

            CloseShape();
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

        public ICommand ClearShapeCommand { get; private set; }

        private void ClearShape(object param)
        {
            this.Items.Clear();
            _points.Clear();
            _lastLine = null;
            _closeLine = null;
            ClosedShape = false;
        }

        #endregion
    }
}
