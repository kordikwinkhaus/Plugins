using System;
using System.Windows;
using System.Windows.Controls;
using ShapeOffset.ViewModels;

namespace ShapeOffset.Views
{
    public class DrawingItemStyleSelector : StyleSelector
    {
        public static DrawingItemStyleSelector Instance = new DrawingItemStyleSelector();

        public override Style SelectStyle(object item, DependencyObject container)
        {
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            if (itemsControl == null) throw new InvalidOperationException("Could not find ItemsControl");

            if (item is LineViewModel)
            {
                return (Style)itemsControl.FindResource("LineItemStyle");
            }
            else if (item is PointViewModel)
            {
                return (Style)itemsControl.FindResource("PointItemStyle");
            }

            return base.SelectStyle(item, container);
        }
    }
}
