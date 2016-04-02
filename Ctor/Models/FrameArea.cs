using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// Vloží štulp do prázdného pole na střed. Pokud rám obsahuje více prázdných polí,
        /// zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Artikl štulpu.</param>
        /// <param name="isLeftSide">Zda-li je štulp levý.</param>
        /// <param name="color">ID barvy.</param>
        public void InsertFalseMullion(string nrArt, bool isLeftSide, int color)
        {
            this.InsertFalseMullion(nrArt, isLeftSide, 0.5f, color);
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
        /// zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Artikl štulpu.</param>
        /// <param name="isLeftSide">Zda-li je štulp levý.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        /// <param name="color">ID barvy.</param>
        public void InsertFalseMullion(string nrArt, bool isLeftSide, float dimX, int color)
        {
            // TODO
        }

        #endregion
    }
}
