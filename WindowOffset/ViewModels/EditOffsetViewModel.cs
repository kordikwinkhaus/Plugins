using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Okna.Plugins.ViewModels;
using WHOkna;

namespace WindowOffset.ViewModels
{
    public class EditOffsetViewModel : ViewModelBase
    {
        private XElement _data;
        private ITopObject _topObject;

        internal EditOffsetViewModel(XElement data, ITopObject topObject)
        {
            _data = data;
            _topObject = topObject;
        }
    }
}
