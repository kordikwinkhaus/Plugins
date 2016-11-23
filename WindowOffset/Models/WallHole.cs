using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using WHOkna;

namespace WindowOffset.Models
{
    /// <summary>
    /// Model díry ve zdi.
    /// </summary>
    internal class WallHole
    {
        private XmlAdapter _xmlData;
        private ITopObject _topObject;
        private SizeF[] _slants;

        internal WallHole(XElement data, ITopObject topObject)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (topObject == null) throw new ArgumentNullException(nameof(topObject));

            _xmlData = new XmlAdapter(data);
            _topObject = topObject;

            WallHoleData wallHoleData = (_xmlData.IsWinOffsetSpecified()) ? _xmlData.GetCurrentData() : GetTopObjectWallHoleData();
            Init(wallHoleData);
        }

        #region Input

        private WallHoleData GetTopObjectWallHoleData()
        {
            SizeF[] slants = new SizeF[4];

            for (int i = 0; i < slants.Length; i++)
            {
                slants[i] = _topObject.get_Slants(i);
            }

            return new WallHoleData
            {
                MainDimension = _topObject.Dimensions.Size,
                Slants = slants
            };
        }

        private void Init(WallHoleData data)
        {
            _slants = data.Slants;

            this.MainOffset = new MainOffset();
            var size = this.Size = data.MainDimension;
            var topLeft = data.Slants[0];
            var topRight = data.Slants[1];
            var bottomRight = data.Slants[2];
            var bottomLeft = data.Slants[3];

            InitOffsetItems(size, topLeft, topRight, bottomRight, bottomLeft);
            InitDimensions(size, topLeft, topRight, bottomRight, bottomLeft);
            ComputeCentroid();
            InitOffsetValues(data.Offsets);
        }

        private void InitOffsetItems(SizeF size, SizeF topLeft, SizeF topRight, SizeF bottomRight, SizeF bottomLeft)
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

        private void InitDimensions(SizeF size, SizeF topLeft, SizeF topRight, SizeF bottomRight, SizeF bottomLeft)
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

        private void InitOffsetValues(IDictionary<int, int> offsets)
        {
            if (offsets != null && offsets.Count != 0)
            {
                int offset;
                if (offsets.TryGetValue(-1, out offset))
                {
                    this.MainOffset.Offset = offset;
                }

                foreach (int id in offsets.Keys.Where(id => id != -1))
                {
                    this.SideOffsets.Single(s => s.Side == id).Offset = offsets[id];
                }
            }
        }

        internal SizeF Size { get; private set; }

        internal MainOffset MainOffset { get; private set; }

        internal IList<SideOffset> SideOffsets { get; } = new List<SideOffset>();

        internal IList<DimensionLayer> LeftDims { get; } = new List<DimensionLayer>();

        internal IList<DimensionLayer> TopDims { get; } = new List<DimensionLayer>();

        internal IList<DimensionLayer> RightDims { get; } = new List<DimensionLayer>();

        internal IList<DimensionLayer> BottomDims { get; } = new List<DimensionLayer>();

        internal PointF Centroid { get; private set; }

        #endregion

        #region Output

        internal WindowOutline GetWindowOutline()
        {
            List<Line> lines = new List<Line>();
            foreach (var sideOffset in this.SideOffsets)
            {
                lines.Add(Line.Create(sideOffset));
            }

            TrimLines(lines);

            RectangleF area = GetBoundingRectangle(lines);

            var result = new WindowOutline
            {
                Size = area.Size
            };
            result.TopLeft = TryGetSlant(lines, 1);
            result.TopRight = TryGetSlant(lines, 3);
            result.BottomRight = TryGetSlant(lines, 5);
            result.BottomLeft = TryGetSlant(lines, 7);

            return result;
        }

        private static void TrimLines(List<Line> lines)
        {
            // "ořeže" přesahy offsetovaných úseček

            while (true)
            {
                for (int i = 1; i < lines.Count; i++)
                {
                    IntersectLines(lines[i - 1], lines[i]);
                }
                IntersectLines(lines.Last(), lines.First());

                bool removedLine = false;
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    if (!lines[i].IsValid())
                    {
                        lines.RemoveAt(i);
                        removedLine = true;
                    }
                }

                if (!removedLine)
                {
                    return;
                }
            }
        }

        private static void IntersectLines(Line line, Line nextLine)
        {
            if (!line.ContinuesWith(nextLine))
            {
                var intersection = line.Intersection(nextLine);
                line.End = intersection;
                nextLine.Start = intersection;
            }
        }

        private static RectangleF GetBoundingRectangle(List<Line> lines)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var line in lines)
            {
                FindMinMax(line.Start, ref minX, ref minY, ref maxX, ref maxY);
                FindMinMax(line.End, ref minX, ref minY, ref maxX, ref maxY);
            }

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        private static void FindMinMax(PointF point, ref float minX, ref float minY, ref float maxX, ref float maxY)
        {
            if (point.X < minX) minX = point.X;
            if (point.Y < minY) minY = point.Y;
            if (point.X > maxX) maxX = point.X;
            if (point.Y > maxY) maxY = point.Y;
        }

        private SizeF TryGetSlant(List<Line> lines, int side)
        {
            foreach (var line in lines)
            {
                if (line.Side == side)
                {
                    return line.GetSlant();
                }
            }
            return SizeF.Empty;
        }

        internal void SaveToPositionData()
        {
            var wallHoleData = GetWallHoleData();
            _xmlData.SetCurrentData(wallHoleData);
        }

        internal WallHoleData GetWallHoleData()
        {
            return new WallHoleData
            {
                MainDimension = this.Size,
                Slants = _slants,
                Offsets = GetOffsets()
            };
        }

        private IDictionary<int, int> GetOffsets()
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            result.Add(-1, this.MainOffset.Offset);

            foreach (var item in this.SideOffsets)
            {
                if (item.HasOwnValue)
                {
                    result.Add(item.Side, item.Offset);
                }
            }

            return result;
        }

        #endregion

    }
}
