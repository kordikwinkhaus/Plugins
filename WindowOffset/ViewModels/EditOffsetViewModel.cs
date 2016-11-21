using System;
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
        private const int WIDTH_MARGIN = 20;
        private const int HEIGHT_MARGIN = 10;

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

            double dimLayerHeight = Math.Round(textHeight + 4, 0);
            double topMargin = _wallHole.TopDims.Count * dimLayerHeight;
            double leftMargin = _wallHole.LeftDims.Count * dimLayerHeight;
            double bottomMargin = _wallHole.BottomDims.Count * dimLayerHeight;
            double rightMargin = _wallHole.BottomDims.Count * dimLayerHeight;

            double availableWidth = actualWidth - leftMargin - rightMargin - 2 * WIDTH_MARGIN;
            double availableHeight = actualHeight - topMargin - bottomMargin - 2 * HEIGHT_MARGIN;

            double xScale = _wallHole.Size.Width / availableWidth;
            double yScale = _wallHole.Size.Height / availableHeight;
            double scale = Math.Max(xScale, yScale);

            double left = leftMargin + WIDTH_MARGIN;
            double top = topMargin + HEIGHT_MARGIN;

            foreach (var item in this.CanvasItems)
            {
                item.Recalculate(scale, left, top);
            }
        }

        public IList<SideOffsetViewModel> OffsetItems { get; } = new List<SideOffsetViewModel>();

        public IList<IScaleable> CanvasItems { get; } = new List<IScaleable>();
    }
}
