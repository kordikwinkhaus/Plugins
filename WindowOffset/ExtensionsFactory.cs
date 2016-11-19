using System.Windows;
using UserExtensions;
using WindowOffset.Views;

namespace UserExt
{
    public class ExtensionsFactory : IExtensionsFactory
    {
        public FrameworkElement GetPropertyPage(EPropPage pg, string connection, string lang)
        {
            switch (pg)
            {
                case EPropPage.pDziura:
                    return new WindowOffsetPage();
            }

            return null;
        }
    }
}
