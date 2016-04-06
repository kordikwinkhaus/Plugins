using System.Drawing;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt plochy v křídle.
    /// </summary>
    public class SashArea : FrameAreaBase
    {
        private readonly Sash _parent;

        internal SashArea(IArea area, Sash parent)
            : base(area)
        {
            _parent = parent;
        }

        #region Insert mullion

        protected override RectangleF GetCorrectedDimensions()
        {
            return _area.Rectangle;
        }

        protected override TArea CreateArea<TArea>(IArea area)
        {
            return new SashArea(area, _parent) as TArea;
        }

        protected override int GetParentColor()
        {
            return _parent.Data.Color;
        }

        #endregion
    }
}
