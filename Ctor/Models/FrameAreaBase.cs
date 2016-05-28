using System;
using System.Drawing;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    public abstract class FrameAreaBase : Area
    {
        internal FrameAreaBase(IArea area)
            : base(area)
        {
        }

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
            return this.InsertHorizontalMullion(nrArt, dimY, GetParentColor());
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
            return this.InsertVerticalMullion(nrArt, dimX, GetParentColor());
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

            if (dim <= 0) throw new ArgumentOutOfRangeException(nameof(dim));

            var parameters = Parameters.ForMullion(nrArt, color);
            var insertionPoint = new PointF();
            switch (direction)
            {
                case EDir.dTop:
                    if (dim < 1)
                    {
                        var dims = GetCorrectedDimensions();
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
                        var dims = GetCorrectedDimensions();
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
            if (top.Update(true) && newParts != null)
            {
                top.CheckPoint();
                top.Invalidate();

                this.Invalidate();

                var result = new MullionInsertionResult<TArea>();
                result.Mullion = new Bar((IBar)newParts[0]);

                result.Area1 = CreateArea<TArea>((IArea)newParts[1]);
                result.Area2 = CreateArea<TArea>(_area);

                return result;
            }
            else
            {
                top.Undo(Strings.CannotInsertMullion);
                top.Invalidate();
                throw new ModelException(Strings.CannotInsertMullion);
            }
        }

        protected virtual RectangleF GetCorrectedDimensions()
        {
            return _area.Rectangle;
        }

        protected abstract TArea CreateArea<TArea>(IArea area) where TArea : Area;

        protected abstract int GetParentColor();

        #endregion

        #region Glasspackets

        /// <summary>
        /// Vloží zadaný paket do tohoto pole.
        /// </summary>
        /// <param name="nrArt">Číslo výrobku paketu.</param>
        public Glasspacket InsertGlasspacket(string nrArt)
        {
            var parameters = Parameters.ForGlasspacket(nrArt);
            _area.AddChild(EProfileType.tSzyba, parameters);
            return CreateGlasspacket((IGlazing)_area.Child);
        }

        protected abstract Glasspacket CreateGlasspacket(IGlazing glazing);

        #endregion
    }
}
