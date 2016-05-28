using System;
using System.Drawing;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt plochy v rámu.
    /// </summary>
    public class FrameArea : FrameAreaBase
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

        protected override Glasspacket CreateGlasspacket(IGlazing glazing)
        {
            return new Glasspacket(glazing, _parent);
        }

        #region Insert false mullion

        /// <summary>
        /// Vloží štulp v barvě rámu do tohoto pole na střed. 
        /// Po vložení štulpu je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu štulpu.</param>
        /// <param name="isLeftSide">Strana montáže štulpu.</param>
        public FalseMullionInsertionResult InsertFalseMullion(string nrArt, bool isLeftSide)
        {
            return this.InsertFalseMullion(nrArt, isLeftSide, 0.5f);
        }

        /// <summary>
        /// Vloží štulp v barvě rámu do tohoto pole. 
        /// Po vložení štulpu je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu štulpu.</param>
        /// <param name="isLeftSide">Strana montáže štulpu.</param>
        /// <param name="dimX">Souřadnice štulpu v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šíři pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public FalseMullionInsertionResult InsertFalseMullion(string nrArt, bool isLeftSide, float dimX)
        {
            return this.InsertFalseMullion(nrArt, isLeftSide, dimX, _parent.Data.Color);
        }

        /// <summary>
        /// Vloží štulp do tohoto pole. 
        /// Po vložení štulpu je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu štulpu.</param>
        /// <param name="isLeftSide">Strana montáže štulpu.</param>
        /// <param name="dimX">Souřadnice štulpu v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šíři pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        /// <param name="color">ID barvy.</param>
        public FalseMullionInsertionResult InsertFalseMullion(string nrArt, bool isLeftSide, float dimX, int color)
        {
            CheckInvalidation();

            if (dimX <= 0) throw new ArgumentOutOfRangeException();

            var parameters = Parameters.ForFalseMullion(nrArt, color, isLeftSide);
            var insertionPoint = new PointF();
            if (dimX < 1)
            {
                var dims = _parent.GetCorrectedDimensions();
                insertionPoint.X = dims.X + (dims.Width * dimX);
            }
            else
            {
                insertionPoint.X = dimX;
            }
            insertionPoint.Y = _area.Rectangle.Y + (_area.Rectangle.Height * 0.5f);

            IPart[] newParts = _area.AddBar(EProfileType.tPrzymyk, EDir.dLeft, insertionPoint, parameters);

            var top = _area.TopObject;
            if (top.Update(true))
            {
                top.CheckPoint();
                top.Invalidate();

                this.Invalidate();
                var recNew = _area.Rectangle;
                var result = new FalseMullionInsertionResult();
                var area1 = new FrameArea((IArea)newParts[1], _parent);
                var area2 = new FrameArea(_area, _parent);

                result.LeftSash = area1.InsertSash();
                result.RightSash = area2.InsertSash();
                result.FalseMullion = new FalseMullion((IFalseMullion)newParts[0]);

                return result;
            }
            else
            {
                top.Undo(Strings.CannotInsertFalseMullion);
                top.Invalidate();
                throw new ModelException(Strings.CannotInsertFalseMullion);
            }
        }

        #endregion

        #region Insert mullion

        protected override RectangleF GetCorrectedDimensions()
        {
            return _parent.GetCorrectedDimensions();
        }

        protected override TArea CreateArea<TArea>(IArea area)
        {
            return new FrameArea(area, _parent) as TArea;
        }

        protected override int GetParentColor()
        {
            return _parent.Data.Color;
        }

        #endregion
    }
}
