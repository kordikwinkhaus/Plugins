using System;
using System.Drawing;
using WHOkna;
using static System.Math;

namespace WindowOffset.Models
{
    internal class WindowOutline
    {
        internal SizeF Size { get; set; }

        internal SizeF TopLeft { get; set; }

        internal SizeF TopRight { get; set; }

        internal SizeF BottomLeft { get; set; }

        internal SizeF BottomRight { get; set; }

        internal WindowOutline Scale(float scaleFactor)
        {
            return new WindowOutline
            {
                Size = Scale(this.Size, scaleFactor),
                TopLeft = Scale(this.TopLeft, scaleFactor),
                TopRight = Scale(this.TopRight, scaleFactor),
                BottomRight = Scale(this.BottomRight, scaleFactor),
                BottomLeft = Scale(this.BottomLeft, scaleFactor)
            };
        }

        private static SizeF Scale(SizeF size, float scaleFactor)
        {
            return new SizeF(size.Width * scaleFactor, size.Height * scaleFactor);
        }

        internal bool ApplyTo(ITopObject topObject)
        {
            // vynulování šikmin
            for (int i = 0; i < 4; i++)
            {
                topObject.set_Slants(i, SizeF.Empty);
            }

            // zaokrouhlení hlavních rozměrů
            float w = (float)Round(this.Size.Width, 0, MidpointRounding.AwayFromZero);
            float h = (float)Round(this.Size.Height, 0, MidpointRounding.AwayFromZero);

            var mainDim = GetRounded(this.Size);

            topObject.Dimensions = new RectangleF(PointF.Empty, mainDim);

            var tl = GetRounded(this.TopLeft);
            var tr = GetRounded(this.TopRight);
            var br = GetRounded(this.BottomRight);
            var bl = GetRounded(this.BottomLeft);

            // validate left
            float sumL = tl.Height + bl.Height;
            if (sumL > mainDim.Height)
            {
                bl.Height = mainDim.Height - tl.Height;
            }

            // validate top
            float sumT = tl.Width + tr.Width;
            if (sumT > mainDim.Width)
            {
                tr.Width = mainDim.Width - tl.Width;
            }

            // validate right
            float sumR = tr.Height + br.Height;
            if (sumR > mainDim.Height)
            {
                br.Height = mainDim.Height - tr.Height;
            }

            // validate bottom
            float sumB = br.Width + bl.Width;
            if (sumB > mainDim.Width)
            {
                br.Width = mainDim.Width - bl.Width;
            }

            topObject.set_Slants(0, tl);
            topObject.set_Slants(1, tr);
            topObject.set_Slants(2, br);
            topObject.set_Slants(3, bl);

            topObject.Update(true);
            topObject.CheckPoint();
            topObject.Invalidate();

            return true;
        }

        private SizeF GetRounded(SizeF size)
        {
            float w = (float)Round(size.Width, 0, MidpointRounding.AwayFromZero);
            float h = (float)Round(size.Height, 0, MidpointRounding.AwayFromZero);

            return new SizeF(w, h);
        }
    }
}
