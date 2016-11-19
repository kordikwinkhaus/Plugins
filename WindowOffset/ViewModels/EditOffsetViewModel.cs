using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Okna.Plugins.ViewModels;
using WHOkna;
using WindowOffset.Properties;

namespace WindowOffset.ViewModels
{
    public class EditOffsetViewModel : ViewModelBase
    {
        private XElement _data;
        private ITopObject _topObject;
        private MainOffsetItemViewModel _mainItem;

        internal EditOffsetViewModel(XElement data, ITopObject topObject)
        {
            _data = data;
            _topObject = topObject;

            CreateItems();
        }

        private void CreateItems()
        {
            _mainItem = new MainOffsetItemViewModel
            {
                Name = Resources.Position,
                Side = -1
            };
            this.Items.Add(_mainItem);

            var size = _topObject.Dimensions;
            var topLeft = _topObject.get_Slants(0);
            var topRight = _topObject.get_Slants(1);
            var bottomRight = _topObject.get_Slants(2);
            var bottomLeft = _topObject.get_Slants(3);

            // left?
            if ((size.Height - topLeft.Height - bottomLeft.Height) > 0)
            {
                AddItem(Resources.Left, 0);
            }

            // top left?
            if (topLeft.Height != 0 && topLeft.Width != 0)
            {
                AddItem(Resources.TopLeft, 1);
            }

            // top?
            if ((size.Width - topLeft.Width - topRight.Width) > 0)
            {
                AddItem(Resources.Top, 2);
            }

            // top right?
            if (topRight.Height != 0 && topRight.Width != 0)
            {
                AddItem(Resources.TopRight, 3);
            }

            // right?
            if ((size.Height - topRight.Height - bottomRight.Height) > 0)
            {
                AddItem(Resources.Right, 4);
            }

            // bottom right
            if (bottomRight.Height != 0 && bottomRight.Width != 0)
            {
                AddItem(Resources.BottomRight, 5);
            }

            // bottom?
            if ((size.Width - bottomLeft.Width - bottomRight.Width)  > 0)
            {
                AddItem(Resources.Bottom, 6);
            }

            // bottom left?
            if (bottomLeft.Height != 0 && bottomLeft.Width != 0)
            {
                AddItem(Resources.BottomLeft, 7);
            }
        }

        private void AddItem(string name, int side)
        {
            var item = new OffsetItemViewModel
            {
                Name = name,
                Side = side
            };
            this.Items.Add(item);
            _mainItem.Add(item);
        }

        public ObservableCollection<OffsetItemViewModel> Items { get; } = new ObservableCollection<OffsetItemViewModel>();
    }
}
