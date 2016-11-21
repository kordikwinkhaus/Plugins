using System;
using System.Windows;
using System.Windows.Controls;
using WindowOffset.ViewModels;

namespace WindowOffset.Views
{
    public class WindowAreaItemStyleSelector : StyleSelector
    {
        private static WindowAreaItemStyleSelector s_instance = new WindowAreaItemStyleSelector();
        public static WindowAreaItemStyleSelector Instance
        {
            get { return s_instance; }
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            if (itemsControl == null) throw new InvalidOperationException("Could not find ItemsControl");

            if (item is MainOffsetViewModel)
            {
                return (Style)itemsControl.FindResource("MainOffsetStyle");
            }
            else if (item is SideOffsetViewModel)
            {
                return (Style)itemsControl.FindResource("SideOffsetStyle");
            }
            else if (item is SideOffsetLineViewModel)
            {
                return (Style)itemsControl.FindResource("SideOffsetLineStyle");
            }
            else if (item is DimensionViewModel)
            {
                return (Style)itemsControl.FindResource("DimensionStyle");
            }

            return base.SelectStyle(item, container);
        }
    }
}
