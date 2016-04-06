using System;
using System.Drawing;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt plochy v pozici.
    /// </summary>
    public class PositionArea : Area
    {
        private readonly Position _parent;

        internal PositionArea(IArea area, Position parent)
            : base(area)
        {
            _parent = parent;
        }

        /// <summary>
        /// Vloží rám do tohoto pole.
        /// </summary>
        /// <param name="type">ID typu.</param>
        /// <param name="color">ID barvy.</param>
        public Frame InsertFrame(int type, int color)
        {
            CheckInvalidation();

            var parameters = Parameters.ForFrameType(type, color);
            _area.AddChild(EProfileType.tOsciez, parameters);

            IFrame frame = _area.FindFrame();
            if (frame != null)
            {
                return new Frame(frame, _parent);
            }

            throw new ModelException(Strings.CannotInsertFrame);
        }

        #region Coupling profile

        /// <summary>
        /// Vloží horizontální spojovací profil do tohoto pole na střed.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// /// <param name="nrArt">Číslo artiklu spojovacího profilu.</param>
        /// <param name="color">ID barvy.</param>
        public CouplingInsertionResult InsertHorizontalCoupling(string nrArt, int color)
        {
            return this.InsertHorizontalCoupling(nrArt, color, 0.5f);
        }

        /// <summary>
        /// Vloží horizontální spojovací profil do tohoto pole.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// /// <param name="nrArt">Číslo artiklu spojovacího profilu.</param>
        /// /// <param name="color">ID barvy.</param>
        /// <param name="dimY">Souřadnice spojovacího profilu v ose Y. Pokud je menší než jedna, bere se relativně vzhledem k výšce pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public CouplingInsertionResult InsertHorizontalCoupling(string nrArt, int color, float dimY)
        {
            return this.InsertCouplingCore(nrArt, color, dimY, EDir.dLeft);
        }

        /// <summary>
        /// Vloží vertikální spojovací profil do tohoto pole na střed.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu spojovacího profilu.</param>
        /// <param name="color">ID barvy.</param>
        public CouplingInsertionResult InsertVerticalCoupling(string nrArt, int color)
        {
            return this.InsertVerticalCoupling(nrArt, color, 0.5f);
        }

        /// <summary>
        /// Vloží vertikální spojovací profil do tohoto pole.
        /// Po vložení sloupku je tato oblast zneplatněna.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu spojovacího profilu.</param>
        /// <param name="color">ID barvy.</param>
        /// <param name="dimX">Souřadnice spojovacího profilu v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šířce pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public CouplingInsertionResult InsertVerticalCoupling(string nrArt, int color, float dimX)
        {
            return this.InsertCouplingCore(nrArt, color, dimX, EDir.dTop);
        }

        private CouplingInsertionResult InsertCouplingCore(string nrArt, int color, float dim, EDir direction)
        {
            CheckInvalidation();

            if (dim <= 0) throw new ArgumentOutOfRangeException(nameof(dim));

            var parameters = Parameters.ForCouplingProfile(nrArt, color);
            var insertionPoint = new PointF();
            switch (direction)
            {
                case EDir.dTop:
                    if (dim < 1)
                    {
                        insertionPoint.X = _area.Rectangle.X + (_area.Rectangle.Width * dim);
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
                        insertionPoint.Y = _area.Rectangle.Y + (_area.Rectangle.Height * dim);
                    }
                    else
                    {
                        insertionPoint.Y = dim;
                    }
                    insertionPoint.X = _area.Rectangle.X + (_area.Rectangle.Width * 0.5f);
                    break;

                default:
                    throw new ModelException(string.Format(Strings.InvalidCouplingProfileOrientation, direction));
            }
            var profileType = (string.IsNullOrEmpty(nrArt)) ? EProfileType.tNullPLacz : EProfileType.tPLacz;

            IPart[] newParts = _area.AddBar(profileType, direction, insertionPoint, parameters);

            var top = _area.TopObject;
            if (top.Update(true) && newParts != null)
            {
                top.CheckPoint();
                top.Invalidate();

                this.Invalidate();

                var result = new CouplingInsertionResult();
                result.Area1 = new PositionArea((IArea)newParts[1], _parent);
                result.Area2 = new PositionArea(_area, _parent);
                result.Coupling = new Bar((IBar)newParts[0]);

                return result;
            }
            else
            {
                top.Undo(Strings.CannotInsertCouplingProfile);
                top.Invalidate();
                throw new ModelException(Strings.CannotInsertCouplingProfile);
            }
        }

        #endregion
    }
}
