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
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
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
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        /// <param name="color">ID barvy.</param>
        public FalseMullionInsertionResult InsertFalseMullion(string nrArt, bool isLeftSide, float dimX, int color)
        {
            CheckInvalidation();

            if (dimX <= 0 || 1 <= dimX) throw new ArgumentOutOfRangeException();

            var origRectangle = _area.Rectangle;

            var parameters = Parameters.ForFalseMullion(nrArt, color, isLeftSide);
            var insertionPoint = new System.Drawing.PointF();
            insertionPoint.X = _area.Rectangle.X + (_area.Rectangle.Width * dimX);
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

        /// <summary>
        /// Vloží horizontální sloupek v barvě rámu do tohoto pole na střed.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        public MullionInsertionResult<FrameArea> InsertHorizontalMullion(string nrArt)
        {
            return this.InsertHorizontalMullion(nrArt, 0.5f);
        }

        /// <summary>
        /// Vloží horizontální sloupek v barvě rámu do tohoto pole.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimY">Relativní souřadnice v ose Y vzhledem k výšce pole.</param>
        public MullionInsertionResult<FrameArea> InsertHorizontalMullion(string nrArt, float dimY)
        {
            return this.InsertHorizontalMullion(nrArt, dimY, _parent.Data.Color);
        }

        /// <summary>
        /// Vloží horizontální sloupek do tohoto pole.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimY">Relativní souřadnice v ose Y vzhledem k výšce pole.</param>
        /// <param name="color">ID barvy.</param>
        public MullionInsertionResult<FrameArea> InsertHorizontalMullion(string nrArt, float dimY, int color)
        {
            return this.InsertMullionCore<FrameArea>(nrArt, dimY, color, EDir.dLeft);
        }

        /// <summary>
        /// Vloží vertikální sloupek v barvě rámu do tohoto pole na střed.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        public MullionInsertionResult<FrameArea> InsertVerticalMullion(string nrArt)
        {
            return this.InsertVerticalMullion(nrArt, 0.5f);
        }

        /// <summary>
        /// Vloží vertikální sloupek v barvě rámu do tohoto pole.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        public MullionInsertionResult<FrameArea> InsertVerticalMullion(string nrArt, float dimX)
        {
            return this.InsertVerticalMullion(nrArt, dimX, _parent.Data.Color);
        }

        /// <summary>
        /// Vloží vertikální sloupek do tohoto pole.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        /// <param name="color">ID barvy.</param>
        public MullionInsertionResult<FrameArea> InsertVerticalMullion(string nrArt, float dimX, int color)
        {
            return this.InsertMullionCore<FrameArea>(nrArt, dimX, color, EDir.dTop);
        }

        private MullionInsertionResult<TArea> InsertMullionCore<TArea>(string nrArt, float dim, int color, EDir direction) where TArea : Area
        {
            CheckInvalidation();

            if (dim <= 0 || 1 <= dim) throw new ArgumentOutOfRangeException();

            var parameters = Parameters.ForMullion(nrArt, color);
            var insertionPoint = new System.Drawing.PointF();
            var origRectangle = _area.Rectangle;
            float dimX = 0.5f, dimY = 0.5f;
            switch (direction)
            {
                case EDir.dTop:
                    dimX = dim;
                    break;

                case EDir.dLeft:
                    dimY = dim;
                    break;

                default:
                    throw new ModelException(string.Format(Strings.InvalidMullionOrientation, direction));
            }
            insertionPoint.X = _area.Rectangle.X + (_area.Rectangle.Width * dimX);
            insertionPoint.Y = _area.Rectangle.Y + (_area.Rectangle.Height * dimY);

            _area.AddBar(EProfileType.tSlupek, direction, insertionPoint, parameters);

            var top = _area.TopObject;
            if (top.Update(true))
            {
                top.CheckPoint();
                top.Invalidate();

                this.Invalidate();

                var result = new MullionInsertionResult<TArea>();

                foreach (IBar bar in _parent.Data.FindParts(EProfileType.tSlupek, true))
                {
                    if (bar.Rectangle.Contains(insertionPoint))
                    {
                        result.Mullion = new Mullion(bar);
                        break;
                    }
                }

                // TODO: dořešit typy
                result.Area1 = _parent.GetArea((origRectangle.X + insertionPoint.X) / 2.0f, (origRectangle.Y + insertionPoint.Y) / 2.0f) as TArea;
                result.Area2 = _parent.GetArea((origRectangle.Right + insertionPoint.X) / 2.0f, (origRectangle.Bottom + insertionPoint.Y) / 2.0f) as TArea;

                return result;
            }
            else
            {
                top.Undo(Strings.CannotInsertMullion);
                top.Invalidate();
                throw new ModelException(Strings.CannotInsertMullion);
            }
        }

        #endregion
    }
}
