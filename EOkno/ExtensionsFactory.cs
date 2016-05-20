using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using EOkno;
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
                List<KomponentaViewModel> komponenty = new List<KomponentaViewModel>();
                List<PovrchovaUpravaViewModel> povrchoveUpravy = new List<PovrchovaUpravaViewModel>();
                List<string> kody = new List<string>();

                string directory = Utils.GetPluginDirectory<DocumentViewModel>();
                string filename = Path.Combine(directory, "EOkno.xml");
                XDocument doc = XDocument.Load(filename);

                foreach (var elem in doc.Root.Element("komponenty").Elements("komponenta"))
                {
                    var komponenta = new KomponentaViewModel(elem.Value, elem.Attribute("material")?.Value, elem.Attribute("prace")?.Value, elem.GetDefault(), vm);

                    if (komponenty.Any(k => k.Material == komponenta.Material && k.Prace == komponenta.Prace))
                    {
                        throw new InvalidOperationException("Nalezen duplicitní kód materiálu nebo práce.");
                    }
                    else
                    {
                        komponenty.Add(komponenta);
                    }
                }

                foreach (var elem in doc.Root.Element("povrchoveUpravy").Elements("povrchovaUprava"))
                {
                    var povrchovaUprava = new PovrchovaUpravaViewModel(elem.Attribute("kod").Value, elem.Attribute("nazev").Value, elem.GetDefault());
                    if (povrchoveUpravy.Any(pu => pu.Kod == povrchovaUprava.Kod))
                    {
                        throw new InvalidOperationException("Nalezen duplicitní kód povrchové úpravy: " + povrchovaUprava.Kod);
                    }
                    else if (povrchovaUprava.IsDefault && vm.PovrchoveUpravy.Any(pu => pu.IsDefault))
                    {
                        throw new InvalidOperationException("Duplicitní označení povrchové úpravy jako výchozí: " + povrchovaUprava.Kod);
                    }
                    else
                    {
                        povrchoveUpravy.Add(povrchovaUprava);
                    }

                    foreach (var odstinElem in elem.Elements("odstin"))
                    {
                        var odstin = new OdstinViewModel(odstinElem.Attribute("kod").Value, odstinElem.Value, odstinElem.GetDefault());
                        if (kody.Contains(odstin.Kod))
                        {
                            throw new InvalidOperationException("Nalezen duplicitní kód odstínu: " + odstin.Kod);
                        }
                        else if (odstin.IsDefault && povrchovaUprava.Odstiny.Any(o => o.IsDefault))
                        {
                            throw new InvalidOperationException("Duplicitní označení odstínu jako výchozího: " + odstin.Kod);
                        }
                        else
                        {
                            povrchovaUprava.Odstiny.Add(odstin);
                            kody.Add(odstin.Kod);
                        }
                    }
                }

                decimal sleva = 0;
                var slevaAttr = doc.Root.Attribute("sleva");
                if (slevaAttr != null &&
                    decimal.TryParse(slevaAttr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out sleva))
                {
                    vm.VychoziSleva = sleva;
                }

                vm.Komponenty.AddRange(komponenty);
                vm.PovrchoveUpravy.AddRange(povrchoveUpravy);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
