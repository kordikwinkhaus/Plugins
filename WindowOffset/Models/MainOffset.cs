using System;
using System.Collections.Generic;

namespace WindowOffset.Models
{
    internal class MainOffset : SideOffset
    {
        private readonly List<SideOffset> _subitems = new List<SideOffset>();

        public MainOffset()
        {
            this.Side = -1;
        }

        internal void Add(SideOffset subitem)
        {
            if (subitem == null) throw new ArgumentNullException(nameof(subitem));
            _subitems.Add(subitem);
        }

        internal override int Offset
        {
            get { return base.Offset; }
            set
            {
                base.Offset = value;
                foreach (var subitem in _subitems)
                {
                    subitem.TrySetParentOffset(value);
                }
            }
        }
    }
}
