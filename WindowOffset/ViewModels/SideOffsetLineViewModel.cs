using Okna.Plugins.ViewModels;
using WindowOffset.Models;

namespace WindowOffset.ViewModels
{
    public class SideOffsetLineViewModel : ViewModelBase
    {
        private readonly SideOffset _model;

        internal SideOffsetLineViewModel(SideOffset model)
        {
            _model = model;
        }

        public float X1
        {
            get { return _model.Start.X / 5 + 20; }
        }

        public float Y1
        {
            get { return _model.Start.Y / 5 + 20; }
        }

        public float X2
        {
            get { return _model.End.X / 5 + 20; }
        }

        public float Y2
        {
            get { return _model.End.Y / 5 + 20; }
        }
    }
}
