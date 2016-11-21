using System.Windows;
using System.Windows.Controls;

namespace WindowOffset.Views
{
    public class WindowArea : ItemsControl
    {
        static WindowArea()
        {
            ItemsPanelProperty.OverrideMetadata(typeof(WindowArea), new FrameworkPropertyMetadata(GetDefaultItemsPanelTemplate()));
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
            return new WindowAreaItem();
        }
    }
}
