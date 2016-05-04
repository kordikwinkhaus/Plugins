using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Okna.Plugins.Controls
{
    public class RoundWrapPanel : WrapPanel
    {
        public double RoundStep
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("RoundStep", typeof(double), typeof(RoundWrapPanel),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(IsStepValid));

        private static bool IsStepValid(object value)
        {
            double num = (double)value;
            return DoubleUtils.IsNaN(num) || (num >= 0.0 && !double.IsPositiveInfinity(num));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            UVSize thisBaseSize = new UVSize(this.Orientation, constraint.Width, constraint.Height);
            UVSize lineSize = new UVSize(this.Orientation);
            UVSize usedSize = new UVSize(this.Orientation);
            double itemWidth = this.ItemWidth;
            double itemHeight = this.ItemHeight;
            double roundStep = this.RoundStep;
            bool itemWidthDefined = !DoubleUtils.IsNaN(itemWidth);
            bool itemHeightDefined = !DoubleUtils.IsNaN(itemHeight);
            bool roundStepDefined = !DoubleUtils.IsNaN(roundStep);
            Size availableSize = new Size(itemWidthDefined ? itemWidth : constraint.Width, itemHeightDefined ? itemHeight : constraint.Height);
            UIElementCollection internalChildren = base.InternalChildren;
            int i = 0;
            int count = internalChildren.Count;
            while (i < count)
            {
                UIElement uIElement = internalChildren[i];
                if (uIElement != null)
                {
                    uIElement.Measure(availableSize);
                    UVSize elementSize = new UVSize(this.Orientation, itemWidthDefined ? itemWidth : uIElement.DesiredSize.Width, itemHeightDefined ? itemHeight : uIElement.DesiredSize.Height);
                    if (roundStepDefined)
                    {
                        elementSize.U = Math.Ceiling(elementSize.U / roundStep) * roundStep;
                    }
                    if (DoubleUtils.GreaterThan(lineSize.U + elementSize.U, thisBaseSize.U))
                    {
                        usedSize.U = Math.Max(lineSize.U, usedSize.U);
                        usedSize.V += lineSize.V;
                        lineSize = elementSize;
                        if (DoubleUtils.GreaterThan(elementSize.U, thisBaseSize.U))
                        {
                            usedSize.U = Math.Max(elementSize.U, usedSize.U);
                            usedSize.V += elementSize.V;
                            lineSize = new UVSize(this.Orientation);
                        }
                    }
                    else
                    {
                        lineSize.U += elementSize.U;
                        lineSize.V = Math.Max(elementSize.V, lineSize.V);
                    }
                }
                i++;
            }
            usedSize.U = Math.Max(lineSize.U, usedSize.U);
            usedSize.V += lineSize.V;
            return new Size(usedSize.Width, usedSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int arranged = 0;
            double itemWidth = this.ItemWidth;
            double itemHeight = this.ItemHeight;
            double roundStep = this.RoundStep;
            double num2 = 0.0;
            double itemU = (this.Orientation == Orientation.Horizontal) ? itemWidth : itemHeight;
            UVSize lineSize = new UVSize(this.Orientation);
            UVSize thisBaseSize = new UVSize(this.Orientation, finalSize.Width, finalSize.Height);
            bool itemWidthDefined = !DoubleUtils.IsNaN(itemWidth);
            bool itemHeightDefined = !DoubleUtils.IsNaN(itemHeight);
            bool roundStepDefined = !DoubleUtils.IsNaN(roundStep);
            bool useItemU = (this.Orientation == Orientation.Horizontal) ? itemWidthDefined : itemHeightDefined;
            UIElementCollection internalChildren = base.InternalChildren;
            int i = 0;
            int count = internalChildren.Count;
            while (i < count)
            {
                UIElement uIElement = internalChildren[i];
                if (uIElement != null)
                {
                    UVSize elementSize = new UVSize(this.Orientation, itemWidthDefined ? itemWidth : uIElement.DesiredSize.Width, itemHeightDefined ? itemHeight : uIElement.DesiredSize.Height);
                    if (roundStepDefined)
                    {
                        elementSize.U = Math.Ceiling(elementSize.U / roundStep) * roundStep;
                    }
                    if (DoubleUtils.GreaterThan(lineSize.U + elementSize.U, thisBaseSize.U))
                    {
                        this.arrangeLine(num2, lineSize.V, arranged, i, useItemU, itemU, roundStepDefined, roundStep);
                        num2 += lineSize.V;
                        lineSize = elementSize;
                        if (DoubleUtils.GreaterThan(elementSize.U, thisBaseSize.U))
                        {
                            double arg_146_1 = num2;
                            double arg_146_2 = elementSize.V;
                            int expr_13C = i;
                            this.arrangeLine(arg_146_1, arg_146_2, expr_13C, i = expr_13C + 1, useItemU, itemU, roundStepDefined, roundStep);
                            num2 += elementSize.V;
                            lineSize = new UVSize(this.Orientation);
                        }
                        arranged = i;
                    }
                    else
                    {
                        lineSize.U += elementSize.U;
                        lineSize.V = Math.Max(elementSize.V, lineSize.V);
                    }
                }
                i++;
            }
            if (arranged < internalChildren.Count)
            {
                this.arrangeLine(num2, lineSize.V, arranged, internalChildren.Count, useItemU, itemU, roundStepDefined, roundStep);
            }
            return finalSize;
        }

        private void arrangeLine(double v, double lineV, int start, int end, bool useItemU, double itemU, bool useRoundStep, double roundStep)
        {
            double num = 0.0;
            bool horizontal = this.Orientation == Orientation.Horizontal;
            UIElementCollection internalChildren = base.InternalChildren;
            for (int i = start; i < end; i++)
            {
                UIElement uIElement = internalChildren[i];
                if (uIElement != null)
                {
                    UVSize elementSize = new UVSize(this.Orientation, uIElement.DesiredSize.Width, uIElement.DesiredSize.Height);
                    double num2 = elementSize.U;
                    if (useItemU)
                    {
                        num2 = itemU;
                    }
                    else if (useRoundStep)
                    {
                        num2 = Math.Ceiling(elementSize.U / roundStep) * roundStep;
                    }
                    uIElement.Arrange(new Rect(horizontal ? num : v, horizontal ? v : num, horizontal ? num2 : lineV, horizontal ? lineV : num2));
                    num += num2;
                }
            }
        }

        [DebuggerDisplay("Width={Width}, Height={Height}")]
        private struct UVSize
        {
            internal double U;

            internal double V;

            private Orientation _orientation;

            internal double Width
            {
                get
                {
                    if (this._orientation != Orientation.Horizontal)
                    {
                        return this.V;
                    }
                    return this.U;
                }
                set
                {
                    if (this._orientation == Orientation.Horizontal)
                    {
                        this.U = value;
                        return;
                    }
                    this.V = value;
                }
            }

            internal double Height
            {
                get
                {
                    if (this._orientation != Orientation.Horizontal)
                    {
                        return this.U;
                    }
                    return this.V;
                }
                set
                {
                    if (this._orientation == Orientation.Horizontal)
                    {
                        this.V = value;
                        return;
                    }
                    this.U = value;
                }
            }

            internal UVSize(Orientation orientation, double width, double height)
            {
                this.U = (this.V = 0.0);
                this._orientation = orientation;
                this.Width = width;
                this.Height = height;
            }

            internal UVSize(Orientation orientation)
            {
                this.U = (this.V = 0.0);
                this._orientation = orientation;
            }
        }
    }
}
