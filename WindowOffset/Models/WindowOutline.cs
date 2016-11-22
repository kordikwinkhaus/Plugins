using System;
using System.Drawing;
using WHOkna;
using static System.Math;

namespace WindowOffset.Models
{
    internal class WindowOutline
    {
        private const float DELTA = 0.001f;

        private bool _wasScaled;

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
                BottomLeft = Scale(this.BottomLeft, scaleFactor),
                _wasScaled = true
            };
        }

        private static SizeF Scale(SizeF size, float scaleFactor)
        {
            return new SizeF(size.Width * scaleFactor, size.Height * scaleFactor);
        }

        internal bool ApplyTo(ITopObject topObject)
        {
            // round dimensions
            var mainDim = GetRounded(this.Size);
            var tl = GetRounded(this.TopLeft);
            var tr = GetRounded(this.TopRight);
            var br = GetRounded(this.BottomRight);
            var bl = GetRounded(this.BottomLeft);

            bool shouldHorScale = false;
            bool shouldVertScale = false;
            
            // test scale for left
            if (ShouldScale(mainDim.Height, tl.Height, bl.Height))
            {
                shouldVertScale = true;
            }

            // test scale for top
            if (ShouldScale(mainDim.Width, tl.Width, tr.Width))
            {
                shouldHorScale = true;
            }

            // test scale for right
            if (ShouldScale(mainDim.Height, tr.Height, br.Height))
            {
                shouldVertScale = true;
            }

            // test scale for bottom
            if (ShouldScale(mainDim.Width, br.Width, bl.Width))
            {
                shouldHorScale = true;
            }

            bool doScale = false;
            if (shouldHorScale || shouldVertScale)
            {
                if (shouldHorScale && shouldVertScale)
                {
                    if (Abs(mainDim.Height - mainDim.Width) < DELTA)
                    {
                        doScale = true;
                    }
                }
                else
                {
                    doScale = true;
                }
            }
            if (doScale && !_wasScaled)
            {
                float scaleFactor = GetScaleFactor((shouldHorScale) ? this.Size.Width : this.Size.Height);
                var newOutline = this.Scale(scaleFactor);
                return newOutline.ApplyTo(topObject);
            }

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

            // vynulování šikmin
            for (int i = 0; i < 4; i++)
            {
                topObject.set_Slants(i, SizeF.Empty);
            }

            // nastavení nových rozměrů 
            topObject.Dimensions = new RectangleF(PointF.Empty, mainDim);
            topObject.set_Slants(0, tl);
            topObject.set_Slants(1, tr);
            topObject.set_Slants(2, br);
            topObject.set_Slants(3, bl);

            if (topObject.Update(true))
            {
                topObject.CheckPoint();
                topObject.Invalidate();
            }
            else
            {
                topObject.Undo(null);
            }

            return true;
        }

        private bool ShouldScale(float totalLen, float len1, float len2)
        {
            if ((totalLen % 2) == 0) return false;
            if (Abs(len1 - len2) > 1) return false;
            if ((totalLen - len1 - len2) > 1) return false;

            return true;
        }

        private static float GetScaleFactor(float dim)
        {
            return GetNearestEven(dim) / dim;
        }

        private static float GetNearestEven(float number)
        {
            return (float)(Round(number / 2.0, 0, MidpointRounding.AwayFromZero) * 2.0);
        }

        private SizeF GetRounded(SizeF size)
        {
            float width = (float)Round(size.Width, 0, MidpointRounding.AwayFromZero);
            float height = (float)Round(size.Height, 0, MidpointRounding.AwayFromZero);

            return new SizeF(width, height);
        }
    }
}
