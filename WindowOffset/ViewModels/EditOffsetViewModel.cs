using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using Okna.Plugins.ViewModels;
using WHOkna;
using WindowOffset.Properties;

namespace WindowOffset.ViewModels
{
    public class EditOffsetViewModel : ViewModelBase
    {
        private XElement _data;
        private ITopObject _topObject;
        private MainOffsetItemViewModel _mainItem;

        internal EditOffsetViewModel(XElement data, ITopObject topObject)
        {
            _data = data;
            _topObject = topObject;

            Init();

            this.TestDim = new DimensionViewModel
            {
                Value = 1000,
                Start = new System.Windows.Point(100, 100),
                End = new System.Windows.Point(400, 100)
            };
        }

        private void Init()
        {
            _mainItem = new MainOffsetItemViewModel
            {
                Name = Resources.Position,
                Side = -1
            };
            this.Items.Add(_mainItem);

            var size = _topObject.Dimensions;
            var topLeft = _topObject.get_Slants(0);
            var topRight = _topObject.get_Slants(1);
            var bottomRight = _topObject.get_Slants(2);
            var bottomLeft = _topObject.get_Slants(3);

            InitOffsetItems(size, topLeft, topRight, bottomRight, bottomLeft);
            InitWindowPoints(size, topLeft, topRight, bottomRight, bottomLeft);
        }

        private void InitOffsetItems(RectangleF size, SizeF topLeft, SizeF topRight, SizeF bottomRight, SizeF bottomLeft)
        {
            // left?
            if ((size.Height - topLeft.Height - bottomLeft.Height) > 0)
            {
                AddItem(Resources.Left, 0);
            }

            // top left?
            if (topLeft.Height != 0 && topLeft.Width != 0)
            {
                AddItem(Resources.TopLeft, 1);
            }

            // top?
            if ((size.Width - topLeft.Width - topRight.Width) > 0)
            {
                AddItem(Resources.Top, 2);
            }

            // top right?
            if (topRight.Height != 0 && topRight.Width != 0)
            {
                AddItem(Resources.TopRight, 3);
            }

            // right?
            if ((size.Height - topRight.Height - bottomRight.Height) > 0)
            {
                AddItem(Resources.Right, 4);
            }

            // bottom right
            if (bottomRight.Height != 0 && bottomRight.Width != 0)
            {
                AddItem(Resources.BottomRight, 5);
            }

            // bottom?
            if ((size.Width - bottomLeft.Width - bottomRight.Width) > 0)
            {
                AddItem(Resources.Bottom, 6);
            }

            // bottom left?
            if (bottomLeft.Height != 0 && bottomLeft.Width != 0)
            {
                AddItem(Resources.BottomLeft, 7);
            }
        }

        private void AddItem(string name, int side)
        {
            var item = new OffsetItemViewModel
            {
                Name = name,
                Side = side
            };
            this.Items.Add(item);
            _mainItem.Add(item);
        }

        private void InitWindowPoints(RectangleF size, SizeF topLeft, SizeF topRight, SizeF bottomRight, SizeF bottomLeft)
        {
            if (topLeft.Height != 0 && topLeft.Width != 0)
            {
                AddPoint(0, topLeft.Height);
                AddPoint(topLeft.Width, 0);
            }
            else
            {
                AddPoint(0, 0);
            }

            if (topRight.Height != 0 && topRight.Width != 0)
            {
                AddPoint(size.Width - topRight.Width, 0);
                AddPoint(size.Width, topRight.Height);
            }
            else
            {
                AddPoint(size.Width, 0);
            }

            if (bottomRight.Height != 0 && bottomRight.Width != 0)
            {
                AddPoint(size.Width, size.Height - bottomRight.Height);
                AddPoint(size.Width - bottomRight.Width, size.Height);
            }
            else
            {
                AddPoint(size.Width, size.Height);
            }

            if (bottomLeft.Height != 0 && bottomLeft.Width != 0)
            {
                AddPoint(bottomLeft.Width, size.Height);
                AddPoint(0, size.Height - bottomLeft.Height);
            }
            else
            {
                AddPoint(0, size.Height);
            }
            CheckLastPoint();
        }

        private void AddPoint(float x, float y)
        {
            if (this.WindowPoints.Count != 0)
            {
                var lastPoint = this.WindowPoints[this.WindowPoints.Count - 1];
                if (lastPoint.X == x && lastPoint.Y == y)
                {
                    return;
                }
            }
            this.WindowPoints.Add(new System.Windows.Point(x, y));
        }

        private void CheckLastPoint()
        {
            if (this.WindowPoints.Count != 0)
            {
                var firstPoint = this.WindowPoints[0];
                var lastPoint = this.WindowPoints[this.WindowPoints.Count - 1];
                if (firstPoint.X == lastPoint.X && firstPoint.Y == lastPoint.Y)
                {
                    this.WindowPoints.RemoveAt(this.WindowPoints.Count - 1);
                }
            }
        }

        public ObservableCollection<OffsetItemViewModel> Items { get; } = new ObservableCollection<OffsetItemViewModel>();

        public PointCollection WindowPoints { get; } = new PointCollection();

        public DimensionViewModel TestDim { get; set; }
    }
}
