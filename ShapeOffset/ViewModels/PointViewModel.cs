namespace ShapeOffset.ViewModels
{
    public class PointViewModel : ItemViewModel
    {
        public PointViewModel(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        private int _X;
        public int X
        {
            get { return _X; }
            set
            {
                if (_X != value)
                {
                    _X = value;
                    OnPropertyChanged("X");
                }
            }
        }

        private int _Y;
        public int Y
        {
            get { return _Y; }
            set
            {
                if (_Y != value)
                {
                    _Y = value;
                    OnPropertyChanged("Y");
                }
            }
        }
    }
}
