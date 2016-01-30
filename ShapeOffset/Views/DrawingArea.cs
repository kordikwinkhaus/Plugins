using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Okna.Plugins;
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

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            var newPoint = Snap.ToGrid(e.GetPosition(this));
            if (!DoubleUtils.Equals(newPoint, this.MousePosition))
            {
                this.MousePosition = newPoint;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this && e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                this.SelectedItem = null;
                e.Handled = true;
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

        public ItemViewModel SelectedItem
        {
            get { return (ItemViewModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ItemViewModel), typeof(DrawingArea), new PropertyMetadata(null, OnSelectedItemPropertyChanged));

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (DrawingArea)d;

            self.UnselectAll();

            var selectedItem = (ItemViewModel)e.NewValue;
            if (selectedItem != null)
            {
                selectedItem.IsSelected = true;
            }
        }

        private void UnselectAll()
        {
            foreach (var item in this.Items)
            {
                var selectable = (ItemViewModel)item;
                if (selectable != null)
                {
                    selectable.IsSelected = false;
                }
            }
        }
    }
}
