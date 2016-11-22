using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowOffset.Models
{
    internal class WindowOutline
    {
        internal SizeF Size { get; set; }

        internal SizeF TopLeft { get; set; }

        internal SizeF TopRight { get; set; }

        internal SizeF BottomLeft { get; set; }

        internal SizeF BottomRight { get; set; }
    }
}
