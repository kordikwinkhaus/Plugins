using System.Windows;

namespace ShapeOffset.ViewModels
{
    internal static class Snap
    {
        internal const int GRID_SIZE = 50;
        internal const int MIDDLE = GRID_SIZE / 2;

        internal static int ToGrid(int coord)
        {
            int snap = coord % GRID_SIZE;
            if (snap >= MIDDLE)
            {
                coord += GRID_SIZE;
            }
            return coord - snap;
        }

        internal static double ToGrid(double coord)
        {
            double snap = coord % GRID_SIZE;
            if (snap >= MIDDLE)
            {
                coord += GRID_SIZE;
            }
            return coord - snap;
        }

        internal static Point ToGrid(Point p)
        {
            return new System.Windows.Point(ToGrid(p.X), ToGrid(p.Y));
        }
    }
}
