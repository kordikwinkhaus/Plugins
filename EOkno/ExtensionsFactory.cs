using System.Windows;
using System.Linq;
using System.Xml.Linq;
using EOkno.ViewModels;
using EOkno.Views;
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
                    var vmDoc = new DocumentViewModel();
                    Nacist(vmDoc);
                    return new DocumentView { DataContext = vmDoc };

                case EPropPage.pDziura:
                    var vmPos = new PositionViewModel();
                    Nacist(vmPos);
                    return new PositionView { DataContext = vmPos };
            }

            return null;
        }

        private void Nacist(DocumentViewModel vm)
        {
            string filename = @"D:\code\projects\whokna\Plugins\EOkno\EOkno.xml";
            XDocument doc = XDocument.Load(filename);

            vm.Komponenty.AddRange(doc.Root.Element("komponenty")
                                           .Elements("komponenta")
                                           .Select(k => new KomponentaViewModel(k.Value, 
                                                        k.Attribute("material")?.Value, 
                                                        k.Attribute("prace")?.Value)));

            vm.PovrchoveUpravy.AddRange(doc.Root.Element("povrchoveUpravy")
                                                .Elements("povrchovaUprava")
                                                .Select(p => new PovrchovaUpravaViewModel(p.Attribute("kod").Value, p.Attribute("nazev").Value)
                                                {
                                                    Odstiny = p.Elements("odstin")
                                                               .Select(o => new OdstinViewModel(o.Attribute("kod").Value, o.Value))
                                                               .ToList()
                                                }));
        }
    }
}
