using System.Drawing;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Sloupek nebo spojovací profil.
    /// </summary>
    public class Bar : Part
    {
        private readonly IBar _bar;

        internal Bar(IBar bar)
            : base(bar)
        {
            _bar = bar;
        }

        /// <summary>
        /// X-ová souřadnice bodu vložení.
        /// </summary>
        public float InsertionPointX
        {
            get { return _bar.Offset.X; }
            set { this.SetInsertionPoint(value, _bar.Offset.Y); }
        }

        /// <summary>
        /// Y-ová souřadnice bodu vložení.
        /// </summary>
        public float InsertionPointY
        {
            get { return _bar.Offset.Y; }
            set { this.SetInsertionPoint(_bar.Offset.X, value); }
        }

        /// <summary>
        /// Nastaví bod vložení. Použijte v případě, kdy chcete změnit obě souřadnice bodu vložení.
        /// </summary>
        /// <param name="x">X-ová souřadnice bodu vložení.</param>
        /// <param name="y">Y-ová souřadnice bodu vložení.</param>
        public void SetInsertionPoint(float x, float y)
        {
            _bar.Offset = new PointF(x, y);

            var top = _bar.TopObject;
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

        /// <summary>
        /// Zarovná prvek nahoru.
        /// </summary>
        public void SlideToTop()
        {
            SetSlideToEdge(EDir.dTop);
        }

        /// <summary>
        /// Zarovná prvek dolu.
        /// </summary>
        public void SlideToBottom()
        {
            SetSlideToEdge(EDir.dBottom);
        }

        /// <summary>
        /// Zarovná prvek doprava.
        /// </summary>
        public void SlideToRight()
        {
            SetSlideToEdge(EDir.dRight);
        }

        /// <summary>
        /// Zarovná prvek doleva.
        /// </summary>
        public void SlideToLeft()
        {
            SetSlideToEdge(EDir.dLeft);
        }

        private void SetSlideToEdge(EDir dir)
        {
            _bar.SlidedToEdge = dir;

            var top = _bar.TopObject;
            if (top.Update(true))
            {
                top.CheckPoint();
                top.Invalidate();
            }
            else
            {
                top.Undo(Strings.CannotAlignItem);
                top.Invalidate();
                throw new ModelException(Strings.CannotAlignItem);
            }
        }
    }
}
