using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Okna.Plugins.ViewModels;

namespace ShapeOffset.ViewModels
{
    public class CanvasViewModel : ViewModelBase
    {
        const int GRID_SIZE = 50;
        const int MIDDLE = GRID_SIZE / 2;

        public CanvasViewModel()
        {
            this.AddPoint(10, 50);
            this.AddPoint(100, 20);
        }

        public ObservableCollection<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

        internal void AddPoint(int x, int y)
        {
            this.Items.Add(new PointViewModel(Snap(x), Snap(y)));
        }

        private int Snap(int coord)
        {
            int snap = coord % GRID_SIZE;

            if (snap <= MIDDLE)
            {
                return coord - snap;
            }
            else
            {
                return coord + GRID_SIZE - snap;
            }
        }
    }
}
