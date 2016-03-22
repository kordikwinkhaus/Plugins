using System.Windows;
using Ctor.ViewModels;
using Ctor.Views;
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
                    var page = new FastInsertPage();
                    page.ViewModel = new FastInsertViewModel(connection, lang);
                    return page;
            }

            return null;
        }
    }
}
