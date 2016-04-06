using System;
using System.Drawing;
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

            var origRectangle = _area.Rectangle;

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
        /// <param name="dimY">Souřadnice sloupku v ose Y. Pokud je menší než jedna, bere se relativně vzhledem k výšce pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public MullionInsertionResult<FrameArea> InsertHorizontalMullion(string nrArt, float dimY)
        {
            return this.InsertHorizontalMullion(nrArt, dimY, _parent.Data.Color);
        }

        /// <summary>
        /// Vloží horizontální sloupek do tohoto pole.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimY">Souřadnice sloupku v ose Y. Pokud je menší než jedna, bere se relativně vzhledem k výšce pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
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
        /// <param name="dimX">Souřadnice sloupku v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šíři pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public MullionInsertionResult<FrameArea> InsertVerticalMullion(string nrArt, float dimX)
        {
            return this.InsertVerticalMullion(nrArt, dimX, _parent.Data.Color);
        }

        /// <summary>
        /// Vloží vertikální sloupek do tohoto pole.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimX">Souřadnice sloupku v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šíři pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        /// <param name="color">ID barvy.</param>
        public MullionInsertionResult<FrameArea> InsertVerticalMullion(string nrArt, float dimX, int color)
        {
            return this.InsertMullionCore<FrameArea>(nrArt, dimX, color, EDir.dTop);
        }

        private MullionInsertionResult<TArea> InsertMullionCore<TArea>(string nrArt, float dim, int color, EDir direction) where TArea : Area
        {
            CheckInvalidation();

            if (dim <= 0) throw new ArgumentOutOfRangeException();

            var parameters = Parameters.ForMullion(nrArt, color);
            var insertionPoint = new PointF();
            var origRectangle = _area.Rectangle;
            switch (direction)
            {
                case EDir.dTop:
                    if (dim < 1)
                    {
                        var dims = _parent.GetCorrectedDimensions();
                        insertionPoint.X = dims.X + (dims.Width * dim);
                    }
                    else
                    {
                        insertionPoint.X = dim;
                    }
                    insertionPoint.Y = _area.Rectangle.Y + (_area.Rectangle.Height * 0.5f);
                    break;

                case EDir.dLeft:
                    if (dim < 1)
                    {
                        var dims = _parent.GetCorrectedDimensions();
                        insertionPoint.Y = dims.Y + (dims.Height * dim);
                    }
                    else
                    {
                        insertionPoint.Y = dim;
                    }
                    insertionPoint.X = _area.Rectangle.X + (_area.Rectangle.Width * 0.5f);
                    break;

                default:
                    throw new ModelException(string.Format(Strings.InvalidMullionOrientation, direction));
            }

            IPart[] newParts = _area.AddBar(EProfileType.tSlupek, direction, insertionPoint, parameters);

            var top = _area.TopObject;
            if (top.Update(true))
            {
                top.CheckPoint();
                top.Invalidate();

                this.Invalidate();

                var result = new MullionInsertionResult<TArea>();
                result.Mullion = new Mullion((IBar)newParts[0]);

                // TODO: dořešit typy
                result.Area1 = new FrameArea((IArea)newParts[1], _parent) as TArea;
                result.Area2 = new FrameArea(_area, _parent) as TArea;

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
