using System.Windows;
using JvOkna.ViewModels;
using JvOkna.Views;
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
                    var vm = new ReklamaceSklaViewModel();
                    var view = new ReklamaceSklaView { DataContext = vm };
#if (DEBUG)
                    return new Okna.Plugins.Interception.InterceptionView(view);
#else
                    return view;
#endif
            }

            return null;
        }
    }
}
