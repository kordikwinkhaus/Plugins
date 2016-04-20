using System.Windows.ViewModels;
using Ctor.Models;
using WHOkna;

namespace Ctor.ViewModels
{
    public class AreaSelectorViewModel : ViewModelBase
    {
        internal AreaSelectorViewModel(IAreaProvider areaProvider)
        {
            this.AreaProvider = areaProvider;
        }

        internal IAreaProvider AreaProvider { get; private set; }

        internal IArea SelectedArea { get; set; }
    }
}
