using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using Okna.Plugins.ViewModels;
using WHOkna;
using WindowOffset.Models;
using WindowOffset.Properties;

namespace WindowOffset.ViewModels
{
    public class EditOffsetViewModel : ViewModelBase
    {
        private readonly XElement _data;
        private readonly ITopObject _topObject;
        private readonly WallHole _wallHole;
        private MainOffsetViewModel _mainItem;

        internal EditOffsetViewModel(XElement data, ITopObject topObject)
        {
            _data = data;
            _topObject = topObject;
            _wallHole = new WallHole(data, topObject);

            Init();
        }

        private void Init()
        {
            _mainItem = new MainOffsetViewModel(_wallHole.MainOffset);
            this.CanvasItems.Add(_mainItem);
            this.OffsetItems.Add(_mainItem);
            foreach (var sideOffset in _wallHole.SideOffsets)
            {
                var sideOffsetVM = new SideOffsetViewModel(sideOffset);
                _mainItem.Add(sideOffsetVM);
                this.CanvasItems.Add(sideOffsetVM);
                this.OffsetItems.Add(sideOffsetVM);
            }

            
        }

        public IList<SideOffsetViewModel> OffsetItems { get; } = new List<SideOffsetViewModel>();

        public IList<SideOffsetViewModel> CanvasItems { get; } = new List<SideOffsetViewModel>();
    }
}
