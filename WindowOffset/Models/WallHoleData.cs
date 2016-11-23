using System.Collections.Generic;
using System.Drawing;

namespace WindowOffset.Models
{
    internal class WallHoleData
    {
        public SizeF MainDimension { get; set; }

        public SizeF[] Slants { get; set; }

        public IDictionary<int, int> Offsets { get; set; }
    }
}
