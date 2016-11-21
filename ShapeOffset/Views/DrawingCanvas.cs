using System.Windows.Controls;
using System.Windows.Media;

namespace ShapeOffset.Views
{
    public class DrawingCanvas : Canvas
    {
        public DrawingCanvas()
        {
            this.Background = Brushes.Transparent;
            this.Width = 5000;
            this.Height = 3000;
        }
    }
}
