using System;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt plochy v rámu.
    /// </summary>
    public class FrameArea : Area
    {
        private readonly Frame _parent;

        internal FrameArea(IArea area, Frame parent)
            : base(area)
        {
            _parent = parent;
        }

        /// <summary>
        /// Vloží křídlo do pole. 
        /// Pokud pole není prázdné, nebo se nepodaří křídlo vložit, vyhodí <see cref="ModelException"/>.
        /// </summary>
        public Sash InsertSash()
        {
            CheckInvalidation();

            if (this.IsEmpty)
            {
                _area.AddChild(EProfileType.tSkrz, null);
                ISash sash = _area.FindSash();
                if (sash != null)
                {
                    return new Sash(sash, _parent);
                }
            }

            throw new ModelException(Strings.CannotInsertSash);
        }

        #region Insert mullion

        /// <summary>
        /// Vloží štulp do prázdného pole na střed. Pokud rám obsahuje více prázdných polí,
        /// zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Artikl štulpu.</param>
        /// <param name="isLeftSide">Zda-li je štulp levý.</param>
        public void InsertFalseMullion(string nrArt, bool isLeftSide)
        {
            this.InsertFalseMullion(nrArt, isLeftSide, 0.5f);
        }

        /// <summary>
        /// Vloží štulp do prázdného pole. Pokud rám obsahuje více prázdných polí,
        /// zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Artikl štulpu.</param>
        /// <param name="isLeftSide">Zda-li je štulp levý.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        public void InsertFalseMullion(string nrArt, bool isLeftSide, float dimX)
        {
            this.InsertFalseMullion(nrArt, isLeftSide, dimX, _parent.Data.Color);
        }

        /// <summary>
        /// Vloží štulp do prázdného pole. Pokud rám obsahuje více prázdných polí,
        /// zobrazí dialog pro výběr pole. Po vložení štulpu je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Artikl štulpu.</param>
        /// <param name="isLeftSide">Zda-li je štulp levý.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        /// <param name="color">ID barvy.</param>
        public void InsertFalseMullion(string nrArt, bool isLeftSide, float dimX, int color)
        {
            CheckInvalidation();

            if (dimX <= 0 || 1 <= dimX) throw new ArgumentOutOfRangeException();
            if (string.IsNullOrEmpty(nrArt)) throw new ArgumentNullException(nameof(nrArt));
            if (color <= 0) throw new ArgumentOutOfRangeException(nameof(color));

            var origRectangle = _area.Rectangle;

            var parameters = Parameters.ForFalseMullion(nrArt, color, isLeftSide);
            var insertionPoint = new System.Drawing.PointF();
            insertionPoint.X = _area.Rectangle.X + (_area.Rectangle.Width * dimX);
            insertionPoint.Y = _area.Rectangle.Y + (_area.Rectangle.Height * 0.5f);

            _area.AddBar(EProfileType.tPrzymyk, EDir.dLeft, insertionPoint, parameters);

            var top = _area.TopObject;
            if (top.Update(true))
            {
                top.CheckPoint();
                top.Invalidate();

                this.Invalidate();

                var area1 = _parent.GetArea((origRectangle.Left + insertionPoint.X) / 2, insertionPoint.Y);
                var area2 = _parent.GetArea((origRectangle.Right + insertionPoint.X) / 2, insertionPoint.Y);

                area1.InsertSash();
                area2.InsertSash();
            }
            else
            {
                top.Undo(Strings.CannotInsertFalseMullion);
                top.Invalidate();
                throw new ModelException(Strings.CannotInsertFalseMullion);
            }
        }

        #endregion
    }
}
