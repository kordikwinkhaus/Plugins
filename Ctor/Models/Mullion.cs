using System.Drawing;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Sloupek.
    /// </summary>
    public class Mullion : Part
    {
        private readonly IBar _mullion;

        internal Mullion(IBar mullion)
            : base(mullion)
        {
            _mullion = mullion;
        }

        /// <summary>
        /// X-ová souřadnice bodu vložení.
        /// </summary>
        public float InsertionPointX
        {
            get { return _mullion.Offset.X; }
            set { this.SetInsertionPoint(value, _mullion.Offset.Y); }
        }

        /// <summary>
        /// Y-ová souřadnice bodu vložení.
        /// </summary>
        public float InsertionPointY
        {
            get { return _mullion.Offset.Y; }
            set { this.SetInsertionPoint(_mullion.Offset.X, value); }
        }

        /// <summary>
        /// Nastaví bod vložení. Použijte v případě, kdy chcete změnit obě souřadnice bodu vložení.
        /// </summary>
        /// <param name="x">X-ová souřadnice bodu vložení.</param>
        /// <param name="y">Y-ová souřadnice bodu vložení.</param>
        public void SetInsertionPoint(float x, float y)
        {
            _mullion.Offset = new PointF(x, y);

            var top = _mullion.TopObject;
            if (top.Update(true))
            {
                top.CheckPoint();
                top.Invalidate();
            }
            else
            {
                top.Undo(Strings.CannotChangeInsertionPoint);
                top.Invalidate();
                throw new ModelException(Strings.CannotChangeInsertionPoint);
            }
        }
    }
}
