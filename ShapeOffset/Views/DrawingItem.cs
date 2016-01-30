using System.Windows.Controls;
using System.Windows.Input;
using Okna.Plugins;
using ShapeOffset.ViewModels;

namespace ShapeOffset.Views
{
    public class DrawingItem : ContentPresenter
    {
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                // selecting of item
                DrawingArea parent = this.GetVisualParent<DrawingArea>();
                CanvasViewModel parentVM = parent.DataContext as CanvasViewModel;
                ItemViewModel selectable = this.DataContext as ItemViewModel;
                if (parent != null && selectable != null)
                {
                    e.Handled = true;
                    if (!selectable.IsSelected && parentVM != null && parentVM.ClosedShape)
                    {
                        parent.SelectedItem = selectable;
                    }
                }
            }
        }
    }
}
