using System.Windows;
using Ctor;
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
                    var dialogFactory = new DialogFactory();
                    dialogFactory.Register<AreaSelectorViewModel, AreaSelectorDialog>();
                    var proxyDialogFactory = new CustomDialogFactory(dialogFactory);
                    var interaction = new InteractionService(proxyDialogFactory);
                    var page = new FastInsertPage(proxyDialogFactory);
                    page.ViewModel = new FastInsertViewModel(connection, lang, interaction);
                    return page;
            }

            return null;
        }
    }
}
