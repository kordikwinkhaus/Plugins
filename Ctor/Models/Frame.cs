using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt rámu.
    /// </summary>
    public class Frame : Part, IAreaProvider
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
                var area = AreaSelector.SelectArea(this);
                return new FrameArea(area, this);
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
        /// <param name="isLeftSide">Strana montáže štulpu.</param>
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
        /// <param name="isLeftSide">Strana montáže štulpu.</param>
        /// <param name="dimX">Souřadnice štulpu v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šíři pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
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
        /// <param name="isLeftSide">Strana montáže štulpu.</param>
        /// <param name="dimX">Souřadnice štulpu v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šíři pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
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
        public MullionInsertionResult<FrameArea> InsertHorizontalMullion(string nrArt)
        {
            var area = GetEmptyArea();
            return area.InsertHorizontalMullion(nrArt);
        }

        /// <summary>
        /// Vloží horizontální sloupek v barvě rámu do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimY">Souřadnice sloupku v ose Y. Pokud je menší než jedna, bere se relativně vzhledem k výšce pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public MullionInsertionResult<FrameArea> InsertHorizontalMullion(string nrArt, float dimY)
        {
            var area = GetEmptyArea();
            return area.InsertHorizontalMullion(nrArt, dimY);
        }

        /// <summary>
        /// Vloží horizontální sloupek do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimY">Souřadnice sloupku v ose Y. Pokud je menší než jedna, bere se relativně vzhledem k výšce pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        /// <param name="color">ID barvy.</param>
        public MullionInsertionResult<FrameArea> InsertHorizontalMullion(string nrArt, float dimY, int color)
        {
            var area = GetEmptyArea();
            return area.InsertHorizontalMullion(nrArt, dimY, color);
        }

        /// <summary>
        /// Vloží vertikální sloupek v barvě rámu do prázdného pole na střed.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        public MullionInsertionResult<FrameArea> InsertVerticalMullion(string nrArt)
        {
            var area = GetEmptyArea();
            return area.InsertVerticalMullion(nrArt);
        }

        /// <summary>
        /// Vloží vertikální sloupek v barvě rámu do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimX">Souřadnice sloupku v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šíři pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public MullionInsertionResult<FrameArea> InsertVerticalMullion(string nrArt, float dimX)
        {
            var area = GetEmptyArea();
            return area.InsertVerticalMullion(nrArt, dimX);
        }

        /// <summary>
        /// Vloží vertikální sloupek do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu sloupku.</param>
        /// <param name="dimX">Souřadnice sloupku v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šíři pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        /// <param name="color">ID barvy.</param>
        public MullionInsertionResult<FrameArea> InsertVerticalMullion(string nrArt, float dimX, int color)
        {
            var area = GetEmptyArea();
            return area.InsertVerticalMullion(nrArt, dimX, color);
        }

        #endregion

        /// <summary>
        /// Vrací plochu rámu. Pokud je rám obklopen spojovacími profily,
        /// zkusí opravit zaokrouhlené rozměry.
        /// </summary>
        public RectangleF GetCorrectedDimensions()
        {
            var original = _frame.Rectangle;
            float left = 0, right = 0, bottom = 0, top = 0; // corrections

            foreach (IBar bar in _frame.TopObject.Bars.Where(b => b.Type != EProfileType.tNullPLacz))
            {
                if (bar.Rectangle.Right == original.Left && bar.Dir == EDir.dTop)
                {
                    left = bar.Rectangle.Right - (bar.Offset.X + bar.Rectangle.Width / 2);
                }
                else if (bar.Rectangle.Left == original.Right && bar.Dir == EDir.dTop)
                {
                    right = bar.Rectangle.Left - (bar.Offset.X - bar.Rectangle.Width / 2);
                }
                else if (bar.Rectangle.Top == original.Bottom && bar.Dir == EDir.dLeft)
                {
                    bottom = bar.Rectangle.Top - (bar.Offset.Y - bar.Rectangle.Height / 2);
                }
                else if (bar.Rectangle.Bottom == original.Top && bar.Dir == EDir.dLeft)
                {
                    top = bar.Rectangle.Bottom - (bar.Offset.Y + bar.Rectangle.Height / 2);
                }
            }

            return new RectangleF(original.X - left, original.Y - top, original.Width + left - right, original.Height + top - bottom);
        }

        IEnumerable<IArea> IAreaProvider.GetEmptyAreas()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IArea> IAreaProvider.GetUsedAreas()
        {
            throw new NotImplementedException();
        }
    }
}
