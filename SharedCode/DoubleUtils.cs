using System;
using System.Windows;

namespace Okna.Plugins
{
    public static class DoubleUtils
    {
        private const double EPSILON = 0.0001;

        public static bool Equals(double a, double b)
        {
            return Math.Abs(a - b) < EPSILON;
        }

        public static bool Equals(Point p1, Point p2)
        {
            return Equals(p1.X, p2.X) && Equals(p1.Y, p2.Y);
        }
    }
}
