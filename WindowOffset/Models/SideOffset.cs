using System.Drawing;

namespace WindowOffset.Models
{
    internal class SideOffset
    {
        // strana v konstrukci 0=levý, 1=lh, 2=horní...
        internal int Side { get; set; }

        internal int _parentOffset;
        private int _offset;
        internal virtual int Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
                this.HasOwnValue = true;
            }
        }

        internal bool HasOwnValue { get; private set; }

        internal void ResetOffset()
        {
            if (this.HasOwnValue)
            {
                _offset = _parentOffset;
                this.HasOwnValue = false;
            }
        }

        internal void TrySetParentOffset(int offset)
        {
            _parentOffset = offset;
            if (!this.HasOwnValue)
            {
                _offset = offset;
            }
        }

        internal PointF Start { get; set; }

        internal PointF End { get; set; }
    }
}
