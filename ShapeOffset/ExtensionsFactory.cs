using System.Windows;
using ShapeOffset;
using ShapeOffset.ViewModels;
using UserExtensions;

namespace UserExt
{
    public class ExtensionsFactory : IExtensionsFactory
    {
        public FrameworkElement GetPropertyPage(EPropPage pg, string connection, string lang)
        {
            switch (pg)
            {
                case EPropPage.pDziura:
                    var vm = new CanvasViewModel();
                    return new ShapeOffsetPage { DataContext = vm };
            }

            return null;
        }
    }
}
