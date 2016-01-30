using System.Windows;
using System.Windows.Media;
using ShapeOffset.ViewModels;

namespace ShapeOffset.Views
{
    public class SnapGrid : FrameworkElement
    {
        private readonly VisualCollection _visuals;
        private DrawingVisual _visual;

        public SnapGrid()
        {
            _visuals = new VisualCollection(this);
            SizeChanged += SnapGrid_SizeChanged;
        }

        private void SnapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateVisual();
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override int VisualChildrenCount
        {
            get { return _visuals.Count; }
        }

        private void UpdateVisual()
        {
            if (_visual != null)
            {
                _visuals.Remove(_visual);
            }

            _visual = new SnapGridVisual(this.ActualWidth, this.ActualHeight);
            _visuals.Add(_visual);
        }

        private class SnapGridVisual : DrawingVisual
        {
            public SnapGridVisual(double width, double height)
            {
                this.VisualEdgeMode = EdgeMode.Aliased;

                using (DrawingContext dc = RenderOpen())
                {
                    Pen pen = new Pen(Brushes.Silver, 1);
                    double step = Snap.GRID_SIZE;

                    double x = 0;
                    for (int i = 1; x < width; i++)
                    {
                        x = i * step;

                        double y = 0;
                        for (int j = 1; y < height; j++)
                        {
                            y = j * step;

                            dc.DrawLine(pen, new Point(x, y - 3), new Point(x, y + 2));
                            dc.DrawLine(pen, new Point(x - 3, y), new Point(x + 2, y));
                        }
                    }
                }
            }
        }
    }
}
