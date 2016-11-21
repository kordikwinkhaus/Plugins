using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Okna.Plugins.ViewModels;
using WHOkna;
using WindowOffset.Models;

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
            _mainItem = new MainOffsetViewModel(_wallHole.MainOffset, _wallHole.Centroid);
            this.CanvasItems.Add(_mainItem);
            this.OffsetItems.Add(_mainItem);
            foreach (var sideOffset in _wallHole.SideOffsets)
            {
                var sideOffsetVM = new SideOffsetViewModel(sideOffset);
                _mainItem.Add(sideOffsetVM);
                this.CanvasItems.Add(sideOffsetVM);
                this.OffsetItems.Add(sideOffsetVM);
                this.CanvasItems.Add(new SideOffsetLineViewModel(sideOffset));
            }


        }

        internal void RecalculateSize(double actualWidth, double actualHeight, double textHeight)
        {
            if (actualWidth == 0) return;
            if (actualHeight == 0) return;
            if (textHeight == 0)
            {
                textHeight = 20;
            }
        }

        public IList<SideOffsetViewModel> OffsetItems { get; } = new List<SideOffsetViewModel>();

        public IList CanvasItems { get; } = new ArrayList();
    }
}
