using System.Windows;
using Ctor.Models;
using Ctor.ViewModels;
using Ctor.Views;
using Okna.Plugins.ViewModels;
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
                    var interaction = new InteractionService(Msg.CAPTION);
                    interaction.Register<AreaSelectorDialog, AreaSelectorViewModel>();
                    var page = new FastInsertPage();
                    page.ViewModel = new FastInsertViewModel(connection, lang, interaction);
                    return page;
            }

            return null;
        }
    }
}
