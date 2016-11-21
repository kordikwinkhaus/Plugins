using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using WHOkna;

namespace WindowOffset.Models
{
    /// <summary>
    /// Model díry ve zdi.
    /// </summary>
    internal class WallHole
    {
        private XElement _data;
        private ITopObject _topObject;

        internal WallHole(XElement data, ITopObject topObject)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (topObject == null) throw new ArgumentNullException(nameof(topObject));

            _data = data;
            _topObject = topObject;

            Init();
        }

        private void Init()
        {
            this.MainOffset = new MainOffset();

            var size = _topObject.Dimensions;
            var topLeft = _topObject.get_Slants(0);
            var topRight = _topObject.get_Slants(1);
            var bottomRight = _topObject.get_Slants(2);
            var bottomLeft = _topObject.get_Slants(3);

            InitOffsetItems(size, topLeft, topRight, bottomRight, bottomLeft);
            InitDimensions(size, topLeft, topRight, bottomRight, bottomLeft);
            ComputeCentroid();
        }

        private void InitOffsetItems(RectangleF size, SizeF topLeft, SizeF topRight, SizeF bottomRight, SizeF bottomLeft)
        {
            // left?
            if ((size.Height - topLeft.Height - bottomLeft.Height) > 0)
            {
                AddItem(0, 0, size.Height - bottomLeft.Height, 0, topLeft.Height);
            }

            // top left?
            if (topLeft.Height != 0 && topLeft.Width != 0)
            {
                AddItem(1, 0, topLeft.Height, topLeft.Width, 0);
            }

            // top?
            if ((size.Width - topLeft.Width - topRight.Width) > 0)
            {
                AddItem(2, topLeft.Width, 0, size.Width - topRight.Width, 0);
            }

            // top right?
            if (topRight.Height != 0 && topRight.Width != 0)
            {
                AddItem(3, size.Width - topRight.Width, 0, size.Width, topRight.Height);
            }

            // right?
            if ((size.Height - topRight.Height - bottomRight.Height) > 0)
            {
                AddItem(4, size.Width, topRight.Height, size.Width, size.Height - bottomRight.Height);
            }

            // bottom right
            if (bottomRight.Height != 0 && bottomRight.Width != 0)
            {
                AddItem(5, size.Width, size.Height - bottomRight.Height, size.Width - bottomRight.Width, size.Height);
            }

            // bottom?
            if ((size.Width - bottomLeft.Width - bottomRight.Width) > 0)
            {
                AddItem(6, size.Width - bottomRight.Width, size.Height, bottomLeft.Width, size.Height);
            }

            // bottom left?
            if (bottomLeft.Height != 0 && bottomLeft.Width != 0)
            {
                AddItem(7, bottomLeft.Width, size.Height, 0, size.Height - bottomLeft.Height);
            }
        }

        private void AddItem(int side, float x1, float y1, float x2, float y2)
        {
            var item = new SideOffset
            {
                Side = side,
                Start = new PointF(x1, y1),
                End = new PointF(x2, y2)
            };
            this.SideOffsets.Add(item);
            this.MainOffset.Add(item);
        }

        private void InitDimensions(RectangleF size, SizeF topLeft, SizeF topRight, SizeF bottomRight, SizeF bottomLeft)
        {
            var topMainLayer = new DimensionLayer();
            this.TopDims.Add(topMainLayer);
            topMainLayer.Add(new Dimension(size.Width));

            var leftMainLayer = new DimensionLayer();
            this.LeftDims.Add(leftMainLayer);
            leftMainLayer.Add(new Dimension(size.Height));

            if (topLeft.Width > 0 || topRight.Width > 0)
            {
                var topSubLayer = new DimensionLayer();
                if (topLeft.Width > 0)
                {
                    topSubLayer.Add(new Dimension(topLeft.Width));
                }
                float rest = size.Width - topLeft.Width - topRight.Width;
                if (rest > 0)
                {
                    topSubLayer.Add(new Dimension(rest));
                }
                if (topRight.Width > 0)
                {
                    topSubLayer.Add(new Dimension(topRight.Width));
                }

                if (topSubLayer.Count > 1)
                {
                    this.TopDims.Add(topSubLayer);
                }
            }

            if (topLeft.Height > 0 || bottomLeft.Height > 0)
            {
                var leftSubLayer = new DimensionLayer();
                if (topLeft.Height > 0)
                {
                    leftSubLayer.Add(new Dimension(topLeft.Height));
                }
                float rest = size.Height - topLeft.Height - bottomLeft.Height;
                if (rest > 0)
                {
                    leftSubLayer.Add(new Dimension(rest));
                }
                if (bottomLeft.Height > 0)
                {
                    leftSubLayer.Add(new Dimension(bottomLeft.Height));
                }

                if (leftSubLayer.Count > 1)
                {
                    this.LeftDims.Add(leftSubLayer);
                }
            }

            if (topRight.Height > 0 || bottomRight.Height > 0)
            {
                var rightSubLayer = new DimensionLayer();
                if (topRight.Height > 0)
                {
                    rightSubLayer.Add(new Dimension(topRight.Height));
                }
                float rest = size.Height - topRight.Height - bottomRight.Height;
                if (rest > 0)
                {
                    rightSubLayer.Add(new Dimension(rest));
                }
                if (bottomRight.Height > 0)
                {
                    rightSubLayer.Add(new Dimension(bottomRight.Height));
                }

                if (rightSubLayer.Count > 1)
                {
                    this.RightDims.Add(rightSubLayer);
                }
            }

            if (bottomRight.Width > 0 || bottomLeft.Width > 0)
            {
                var bottomSubLayer = new DimensionLayer();
                if (bottomLeft.Width > 0)
                {
                    bottomSubLayer.Add(new Dimension(bottomLeft.Width));
                }
                float rest = size.Width - bottomLeft.Width - bottomRight.Width;
                if (rest > 0)
                {
                    bottomSubLayer.Add(new Dimension(rest));
                }
                if (bottomRight.Width > 0)
                {
                    bottomSubLayer.Add(new Dimension(bottomRight.Width));
                }

                if (bottomSubLayer.Count > 1)
                {
                    this.BottomDims.Add(bottomSubLayer);
                }
            }
        }

        private void ComputeCentroid()
        {
            List<PointF> vertices = new List<PointF>();

            foreach (var side in this.SideOffsets)
            {
                vertices.Add(side.Start);
            }
            vertices.Add(this.SideOffsets[0].Start);

            this.Centroid = Polygon.ComputeCentroid(vertices);
        }

        internal MainOffset MainOffset { get; private set; }

        internal IList<SideOffset> SideOffsets { get; } = new List<SideOffset>();

        internal IList<DimensionLayer> LeftDims { get; } = new List<DimensionLayer>();

        internal IList<DimensionLayer> TopDims { get; } = new List<DimensionLayer>();

        internal IList<DimensionLayer> RightDims { get; } = new List<DimensionLayer>();

        internal IList<DimensionLayer> BottomDims { get; } = new List<DimensionLayer>();

        internal PointF Centroid { get; private set; }
    }
}
