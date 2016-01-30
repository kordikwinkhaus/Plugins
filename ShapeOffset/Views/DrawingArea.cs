using System.Windows;
using System.Windows.Controls;
using ShapeOffset.ViewModels;

namespace ShapeOffset.Views
{
    public class DrawingArea : ItemsControl
    {
        static DrawingArea()
        {
            ItemsPanelProperty.OverrideMetadata(typeof(DrawingArea), new FrameworkPropertyMetadata(GetDefaultItemsPanelTemplate()));
        }

        private static ItemsPanelTemplate GetDefaultItemsPanelTemplate()
        {
            ItemsPanelTemplate template = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(DrawingCanvas)));
            template.Seal();
            return template;
        }

        public DrawingArea()
        {
            PreviewMouseMove += DrawingArea_PreviewMouseMove;
        }

        private void DrawingArea_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var newPoint = Snap.ToGrid(e.GetPosition(this));
            if (newPoint != this.MousePosition)
            {
                this.MousePosition = newPoint;
            }
        }

        public Point MousePosition
        {
            get { return (Point)GetValue(MousePositionProperty); }
            set { SetValue(MousePositionProperty, value); }
        }

        public static readonly DependencyProperty MousePositionProperty =
            DependencyProperty.Register("MousePosition", typeof(Point), typeof(DrawingArea), new PropertyMetadata(new Point()));

        protected override bool HandlesScrolling
        {
            get { return false; }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DrawingItem();
        }
    }
}
