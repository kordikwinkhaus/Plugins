using System.Windows;
using Union;
using UserExtensions;

namespace UserExt
{
    public class ExtensionsFactory : IExtensionsFactory
    {
        public FrameworkElement GetPropertyPage(EPropPage pg, string connection, string lang)
        {
            switch (pg)
            {
                case EPropPage.pDokument:
                    return new VolbyZakazky();
            }

            return null;
        }
    }
}
