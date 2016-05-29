using System.Windows;
using Janosik.ViewModels;
using Janosik.Views;
using UserExtensions;

namespace UserExt
{
    public class ExtensionsFactory : IExtensionsFactory
    {
        public FrameworkElement GetPropertyPage(EPropPage pg, string connection, string lang)
        {
            switch (pg)
            {
                case EPropPage.pSzyba:
                    return new GlasspacketView { DataContext = new GlasspacketViewModel() };
            }

            return null;
        }
    }
}
