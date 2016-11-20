using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowOffset.ViewModels
{
    public class DimensionViewModel
    {
        public int Value { get; internal set; }

        public Point Start { get; internal set; }

        public Point End { get; internal set; }

        public double Length
        {
            get
            { 
                if (Start.X == End.X)
                {
                    return End.Y - Start.Y;
                }
                else
                {
                    return End.X - Start.X;
                }
            }
        }
    }
}
