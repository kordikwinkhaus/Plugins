using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Okna.Plugins;

namespace ShapeOffset.Views
{
    public class DrawingCanvas : ItemsControl
    {
        const int GRID_SIZE = 20;
        const int MIDDLE = GRID_SIZE / 2;

        static DrawingCanvas()
        {
            ItemsPanelProperty.OverrideMetadata(typeof(DrawingCanvas), new FrameworkPropertyMetadata(GetDefaultItemsPanelTemplate()));
        }

        private static ItemsPanelTemplate GetDefaultItemsPanelTemplate()
        {
            ItemsPanelTemplate template = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas)));
            template.Seal();
            return template;
        }

        protected override bool HandlesScrolling
        {
            get { return false; }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ConstructionPoint();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var size = base.MeasureOverride(constraint);

            Canvas canvas = this.GetVisualChild<Canvas>();
            if (canvas != null)
            {
                canvas.Width = (size.Width < 1) ? 1000 : size.Width;
                canvas.Height = (size.Height < 1) ? 1000 : size.Height;
                canvas.Background = Brushes.Transparent;
            }

            return size;
        }
    }
}
