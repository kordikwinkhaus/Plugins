using System.Windows;
using Imager.Views;
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
                    return new ImagerPage();
            }

            return null;
        }
    }
}
