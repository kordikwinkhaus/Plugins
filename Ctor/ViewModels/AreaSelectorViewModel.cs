using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ctor.Models;
using Okna.Documents.ViewModels;
using WHOkna;

namespace Ctor.ViewModels
{
    public class AreaSelectorViewModel : ViewModelBase
    {
        private IAreaProvider _areaProvider;

        internal AreaSelectorViewModel(IAreaProvider areaProvider)
        {
            _areaProvider = areaProvider;
        }

        public IArea SelectedArea { get; internal set; }
    }
}
