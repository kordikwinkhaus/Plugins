using System;
using System.Drawing;
using static System.Math;

namespace WindowOffset.Models
{
    internal class Line
    {
        private const float DELTA = 0.00001f;

        internal static Line Create(SideOffset model)
        {
            switch (model.Side)
            {
                case 0:
                    if (Abs(model.Start.X - model.End.X) > DELTA) throw new ArgumentException();
                    if (model.Start.Y < model.End.Y) throw new ArgumentException();
                    break;

                case 1:
                    if (model.Start.X > model.End.X) throw new ArgumentException();
                    if (model.Start.Y < model.End.Y) throw new ArgumentException();
                    break;

                case 2:
                    if (Abs(model.Start.Y - model.End.Y) > DELTA) throw new ArgumentException();
                    if (model.Start.X > model.End.X) throw new ArgumentException();
                    break;

                case 3:
                    if (model.Start.X > model.End.X) throw new ArgumentException();
                    if (model.Start.Y > model.End.Y) throw new ArgumentException();
                    break;

                case 4:
                    if (Abs(model.Start.X - model.End.X) > DELTA) throw new ArgumentException();
                    if (model.Start.Y > model.End.Y) throw new ArgumentException();
                    break;

                case 5:
                    if (model.Start.X < model.End.X) throw new ArgumentException();
                    if (model.Start.Y > model.End.Y) throw new ArgumentException();
                    break;

                case 6:
                    if (Abs(model.Start.Y - model.End.Y) > DELTA) throw new ArgumentException();
                    if (model.Start.X < model.End.X) throw new ArgumentException();
                    break;

                case 7:
                    if (model.Start.X < model.End.X) throw new ArgumentException();
                    if (model.Start.Y < model.End.Y) throw new ArgumentException();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Offset(model);
        }

        internal static Line Offset(SideOffset model)
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

        private Line(int side, PointF start, PointF end)
        {
            this.Side = side;
            this.Start = start;
            this.End = end;
        }

        internal int Side { get; private set; }

        internal PointF Start { get; set; }

        internal PointF End { get; set; }
    }
}
