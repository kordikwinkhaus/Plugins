using System;
using System.Collections.Generic;
using System.Linq;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt rámu.
    /// </summary>
    public class Frame : Part
    {
        private readonly IFrame _frame;
        private readonly Position _parent;

        internal Frame(IFrame frame, Position parent)
            : base(frame)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _frame = frame;
            _parent = parent;
        }

        internal IFrame Data
        {
            get { return _frame; }
        }

        /// <summary>
        /// Vrací pozici, ve které je tento rám.
        /// </summary>
        public Position Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Vrací ID rámu (pořadová čísla od jedné).
        /// </summary>
        public int ID
        {
            get { return _frame.GetNumber(EProfileType.tOsciez); }
        }

        /// <summary>
        /// ID typu okna.
        /// </summary>
        public int WindowTypeID
        {
            get { return _frame.FrameType; }
        }

        /// <summary>
        /// Vrací oblast na zadané souřadnici.
        /// </summary>
        /// <param name="pointInAreaX">X-ová souřadnice bodu pro výběr pole.</param>
        /// <param name="pointInAreaY">Y-ová souřadnice bodu pro výběr pole.</param>
        public FrameArea GetArea(float pointInAreaX, float pointInAreaY)
        {
            var area = _frame.GetArea(pointInAreaX, pointInAreaY);
            if (area != null)
            {
                return new FrameArea(area, this);
            }
            
            string msg = string.Format(Strings.CannotFindAreaInFrame, this.ID, pointInAreaX, pointInAreaY);
            throw new ModelException(msg);
        }

        /// <summary>
        /// Vrací prázdnou oblast. Pokud existuje více oblastí, zobrazí dialog pro výběr.
        /// </summary>
        public FrameArea GetEmptyArea()
        {
            var areas = _frame.Areas.Where(a => a.Child == null).ToList();
            if (areas.Count == 1)
            {
                return new FrameArea(areas[0], this);
            }
            else if (areas.Count > 1)
            {
                // TODO: zobrazit dialog; storno z dialogu by mělo vyvolat systemexit exception
                // + udělat interface pro výběr oblastí (jeden dialog vládne všem);
                // interface pak implementovat explicitně
            }

            throw new ModelException(Strings.NoEmptyAreaInFrame);
        }

        /// <summary>
        /// Vloží křídla do prázdných polí.
        /// </summary>
        public void InsertSashes()
        {
            foreach (IArea area in _frame.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tSkrz, null);
            }
        }

        /// <summary>
        /// Vloží křídlo do prázdného pole. Pokud rám obsahuje více prázdných polí, 
        /// zobrazí dialog pro výběr pole.
        /// </summary>
        public Sash InsertSash()
        {
            var area = this.GetEmptyArea();
            return area.InsertSash();
        }

        /// <summary>
        /// Vrací všechna křídla.
        /// </summary>
        /// <returns>Kolekce křídel.</returns>
        public IEnumerable<Sash> GetSashes()
        {
            foreach (ISash sash in _frame.FindParts(EProfileType.tSkrz, false))
            {
                yield return new Sash(sash, this);
            }
        }

        /// <summary>
        /// Vrací křídlo pro zadaný index.
        /// </summary>
        /// <param name="id">Index křídla.</param>
        public Sash this[int id]
        {
            get { return GetSash(id); }
        }

        /// <summary>
        /// Vrací křídlo pro zadaný index.
        /// </summary>
        /// <param name="id">Index křídla.</param>
        public Sash GetSash(int id)
        {
            foreach (ISash sash in _frame.FindParts(EProfileType.tSkrz, false))
            {
                int number = sash.GetNumber(EProfileType.tSkrz);
                if (id == number)
                {
                    return new Sash(sash, this);
                }
            }

            string msg = string.Format(Strings.NoSashInFrame, this.ID, id);
            throw new ModelException(msg);
        }

        /// <summary>
        /// Vloží zadaný paket do prázdných polí rámu a křídel v něm obsažených.
        /// </summary>
        /// <param name="nrArt">Číslo výrobku paketu.</param>
        public void InsertGlasspackets(string nrArt)
        {
            InsertGlasspackets(Parameters.ForGlasspacket(nrArt));
        }

        internal void InsertGlasspackets(Dictionary<string, object> parameters)
        {
            foreach (var sash in GetSashes())
            {
                sash.InsertGlasspackets(parameters);
            }

            foreach (IArea area in _frame.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tSzyba, parameters);
            }
        }

        #region FalseMullion(s)

        /// <summary>
        /// Vloží štulp v barvě rámu do prázdného pole na střed. 
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu štulpu.</param>
        /// <param name="isLeftSide">Zda-li je štulp levý.</param>
        public FalseMullionInsertionResult InsertFalseMullion(string nrArt, bool isLeftSide)
        {
            var area = GetEmptyArea();
            return area.InsertFalseMullion(nrArt, isLeftSide);
        }

        /// <summary>
        /// Vloží štulp v barvě rámu do prázdného pole. 
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu štulpu.</param>
        /// <param name="isLeftSide">Zda-li je štulp levý.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        public FalseMullionInsertionResult InsertFalseMullion(string nrArt, bool isLeftSide, float dimX)
        {
            var area = GetEmptyArea();
            return area.InsertFalseMullion(nrArt, isLeftSide, dimX);
        }

        /// <summary>
        /// Vloží štulp do prázdného pole. 
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu štulpu.</param>
        /// <param name="isLeftSide">Zda-li je štulp levý.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        /// <param name="color">ID barvy.</param>
        public FalseMullionInsertionResult InsertFalseMullion(string nrArt, bool isLeftSide, float dimX, int color)
        {
            var area = GetEmptyArea();
            return area.InsertFalseMullion(nrArt, isLeftSide, dimX, color);
        }

        internal IFalseMullion FindFalseMullion(ISash sash)
        {
            foreach (IFalseMullion falseMullion in _frame.FindParts(EProfileType.tPrzymyk, true))
            {
                if (sash.Rectangle.IntersectsWith(falseMullion.Rectangle))
                {
                    return falseMullion;
                }
            }

            return null;
        }

        #endregion

        #region InsertMullion

        /// <summary>
        /// Vloží horizontální sloupek v barvě rámu do prázdného pole na střed.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        public void InsertHorizontalMullion(string nrArt)
        {
            var area = GetEmptyArea();
            area.InsertHorizontalMullion(nrArt);
        }

        /// <summary>
        /// Vloží horizontální sloupek v barvě rámu do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimY">Relativní souřadnice v ose Y vzhledem k výšce pole.</param>
        public void InsertHorizontalMullion(string nrArt, float dimY)
        {
            var area = GetEmptyArea();
            area.InsertHorizontalMullion(nrArt, dimY);
        }

        /// <summary>
        /// Vloží horizontální sloupek do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimY">Relativní souřadnice v ose Y vzhledem k výšce pole.</param>
        /// <param name="color">ID barvy.</param>
        public void InsertHorizontalMullion(string nrArt, float dimY, int color)
        {
            var area = GetEmptyArea();
            area.InsertHorizontalMullion(nrArt, dimY, color);
        }

        /// <summary>
        /// Vloží vertikální sloupek v barvě rámu do prázdného pole na střed.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        public void InsertVerticalMullion(string nrArt)
        {
            var area = GetEmptyArea();
            area.InsertVerticalMullion(nrArt);
        }

        /// <summary>
        /// Vloží vertikální sloupek v barvě rámu do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        public void InsertVerticalMullion(string nrArt, float dimX)
        {
            var area = GetEmptyArea();
            area.InsertVerticalMullion(nrArt, dimX);
        }

        /// <summary>
        /// Vloží vertikální sloupek do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimX">Relativní souřadnice v ose X vzhledem k šíři pole.</param>
        /// <param name="color">ID barvy.</param>
        public void InsertVerticalMullion(string nrArt, float dimX, int color)
        {
            var area = GetEmptyArea();
            area.InsertVerticalMullion(nrArt, dimX, color);
        }

        #endregion
    }
}
