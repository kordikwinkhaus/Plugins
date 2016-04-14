using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using EOkno.ViewModels;
using EOkno.Views;
using Okna.Plugins;
using Okna.Plugins.Interception;
using UserExtensions;

namespace UserExt
{
    public class ExtensionsFactory : IExtensionsFactory
    {
        private static MessageBroker s_broker = new MessageBroker();

        public FrameworkElement GetPropertyPage(EPropPage pg, string connection, string lang)
        {
            switch (pg)
            {
                case EPropPage.pDokument:
                    var vmDoc = new DocumentViewModel();
                    Nacist(vmDoc);
                    vmDoc.Broker = s_broker;
                    var docView = new DocumentView { DataContext = vmDoc };
#if (DEBUG)
                    return new InterceptionView(docView, true);
#else
                    return docView;
#endif

                case EPropPage.pDziura:
                    var vmPos = new PositionViewModel();
                    Nacist(vmPos);
                    s_broker.Position = vmPos;
                    var posView = new PositionView { DataContext = vmPos };
#if (DEBUG)
                    return new InterceptionView3(posView);
#else
                    return posView;
#endif
            }

            return null;
        }

        internal static void Nacist(ColorsAndComponentsViewModel vm)
        {
            try
            {
                string directory = Utils.GetPluginDirectory<DocumentViewModel>();
                string filename = Path.Combine(directory, "EOkno.xml");
                XDocument doc = XDocument.Load(filename);

                vm.Komponenty.AddRange(doc.Root.Element("komponenty")
                                               .Elements("komponenta")
                                               .Select(k => new KomponentaViewModel(k.Value,
                                                            k.Attribute("material")?.Value,
                                                            k.Attribute("prace")?.Value,
                                                            vm)));

                vm.PovrchoveUpravy.AddRange(doc.Root.Element("povrchoveUpravy")
                                                    .Elements("povrchovaUprava")
                                                    .Select(p => new PovrchovaUpravaViewModel(p.Attribute("kod").Value, p.Attribute("nazev").Value)
                                                    {
                                                        Odstiny = p.Elements("odstin")
                                                                   .Select(o => new OdstinViewModel(o.Attribute("kod").Value, o.Value))
                                                                   .ToList()
                                                    }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
