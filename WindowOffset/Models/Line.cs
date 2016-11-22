using System;
using System.Diagnostics;
using System.Drawing;
using static System.Math;

namespace WindowOffset.Models
{
    [DebuggerDisplay("{Start}  ->  {End}")]
    internal class Line
    {
        private const float DELTA = 0.001f;

        internal static Line Create(SideOffset model)
        {
            if (!IsValidLineSide(model.Start, model.End, model.Side))
            {
                throw new ArgumentException();
            }

            return LineOffset(model);
        }

        private static bool IsValidLineSide(PointF start, PointF end, int side)
        {
            switch (side)
            {
                case 0:
                    if (Abs(start.X - end.X) > DELTA) return false;
                    if (start.Y < end.Y) return false;
                    break;

                case 1:
                    if (start.X > end.X) return false;
                    if (start.Y < end.Y) return false;
                    break;

                case 2:
                    if (Abs(start.Y - end.Y) > DELTA) return false;
                    if (start.X > end.X) return false;
                    break;

                case 3:
                    if (start.X > end.X) return false;
                    if (start.Y > end.Y) return false;
                    break;

                case 4:
                    if (Abs(start.X - end.X) > DELTA) return false;
                    if (start.Y > end.Y) return false;
                    break;

                case 5:
                    if (start.X < end.X) return false;
                    if (start.Y > end.Y) return false;
                    break;

                case 6:
                    if (Abs(start.Y - end.Y) > DELTA) return false;
                    if (start.X < end.X) return false;
                    break;

                case 7:
                    if (start.X < end.X) return false;
                    if (start.Y < end.Y) return false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }

            return true;
        }

        private static Line LineOffset(SideOffset model)
        {
            float x1 = model.Start.X;
            float x2 = model.End.X;
            float y1 = model.Start.Y;
            float y2 = model.End.Y;

            double len = Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            float offsetPixels = model.Offset;

            float x1p = (float)(x1 - offsetPixels * (y2-y1) / len);
            float x2p = (float)(x2 - offsetPixels * (y2-y1) / len);
            float y1p = (float)(y1 - offsetPixels * (x1-x2) / len);
            float y2p = (float)(y2 - offsetPixels * (x1-x2) / len);

            return new Line(model.Side, new PointF(x1p, y1p), new PointF(x2p, y2p));
        }

        internal Line(int side, PointF start, PointF end)
        {
            this.Side = side;
            this.Start = start;
            this.End = end;
        }

        internal int Side { get; private set; }

        internal PointF Start { get; set; }

        internal PointF End { get; set; }

        internal PointF Intersection(Line other)
        {
            // rovnice přímky: Ax + By = C

            float A1 = this.GetA();
            float B1 = this.GetB();
            float C1 = this.GetC();

            float A2 = other.GetA();
            float B2 = other.GetB();
            float C2 = other.GetC();

            float determinant = A1 * B2 - A2 * B1;

            if (determinant == 0)
            {
                // parallel lines
                throw new InvalidOperationException();
            }
            else
            {
                float x = (B2 * C1 - B1 * C2) / determinant;
                float y = (A1 * C2 - A2 * C1) / determinant;
                return new PointF(x, y);
            }
        }

        private float GetA()
        {
            return this.End.Y - this.Start.Y;
        }

        private float GetB()
        {
            return this.Start.X - this.End.X;
        }

        private float GetC()
        {
            return this.GetA() * this.Start.X + this.GetB() * this.Start.Y;
        }

        internal SizeF GetSlant()
        {
            float width = Abs(this.Start.X - this.End.X);
            float height = Abs(this.Start.Y - this.End.Y);

            return new SizeF(width, height);
        }

        internal bool IsValid()
        {
            return IsValidLineSide(this.Start, this.End, this.Side);
        }

        internal bool ContinuesWith(Line other)
        {
            if (Abs(this.End.X - other.Start.X) > DELTA) return false;
            if (Abs(this.End.Y - other.Start.Y) > DELTA) return false;

            return true;
        }
    }
}
