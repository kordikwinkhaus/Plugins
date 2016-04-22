using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt křídla.
    /// </summary>
    public class Sash : Part, IAreaProvider
    {
        private readonly ISash _sash;
        private readonly Frame _parent;

        internal Sash(ISash sash, Frame parent)
            : base(sash)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _sash = sash;
            _parent = parent;
        }

        internal ISash Data
        {
            get { return _sash; }
        }

        /// <summary>
        /// Vrací rám, ve kterém je toto křídlo.
        /// </summary>
        public Frame Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Vrací ID křídla (pořadová čísla od jedné).
        /// </summary>
        public int ID
        {
            get { return _sash.GetNumber(EProfileType.tSkrz); }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{ID=")
              .Append(this.ID)
              .Append(", Width=")
              .Append(this.Width)
              .Append(", Height=")
              .Append(this.Height)
              .Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Vloží zadaný paket do prázdných polí křídla.
        /// </summary>
        /// <param name="nrArt">Číslo výrobku paketu.</param>
        public void InsertGlasspackets(string nrArt)
        {
            InsertGlasspackets(Parameters.ForGlasspacket(nrArt));
        }

        internal void InsertGlasspackets(Dictionary<string, object> parameters)
        {
            foreach (IArea area in _sash.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tSzyba, parameters);
            }
        }

        public IList<Glasspacket> GetGlasspackets()
        {
            List<Glasspacket> result = new List<Glasspacket>();
            foreach (IGlazing glazing in _sash.FindParts(EProfileType.tSzyba, false))
            {
                result.Add(new Glasspacket(glazing, this));
            }
            return result;
        }

        /// <summary>
        /// Vloží kování do křídla.
        /// </summary>
        /// <param name="type">ID typu kování.</param>
        public void InsertFittings(int type)
        {
            this.InsertFittingsCore(type, null);
        }

        /// <summary>
        /// Vloží kování do křídla.
        /// </summary>
        /// <param name="type">ID typu kování.</param>
        /// <param name="inversion">Zda-li použít inverzi.</param>
        public void InsertFittings(int type, bool inversion)
        {
            this.InsertFittingsCore(type, inversion);
        }

        private void InsertFittingsCore(int type, bool? inversion)
        {
            var doc = _sash.TopObject.Position.Document;
            bool oldUpdateTechnology = doc.UpdateTechnology;
            int oldFittingsType = _sash.FittingsType;
            bool oldFittingsInversion = _sash.FittingsInversion;

            try
            {
                doc.UpdateTechnology = true;

                _sash.FittingsType = type;
                // ISash.FittingsInversionEnabled není implementováno
                //if (_sash.FittingsInversionEnabled)
                //{
                if (inversion.HasValue)
                {
                    _sash.FittingsInversion = inversion.Value;
                }
                //}

                if (_sash.Update(true))
                {
                    _sash.TopObject.CheckPoint();
                    _sash.TopObject.Invalidate();
                }
                else
                {
                    _sash.FittingsType = oldFittingsType;
                    _sash.FittingsInversion = oldFittingsInversion;
                    _sash.Update(true);
                }
            }
            finally
            {
                doc.UpdateTechnology = oldUpdateTechnology;
            }
        }

        /// <summary>
        /// Vrací parametry pro vyhledání výběru kování.
        /// </summary>
        public FittingsFindArgs GetFittingsFindArgs()
        {
            FittingsFindArgs result = new FittingsFindArgs
            {
                WindowTypeID = _parent.WindowTypeID
            };

            var falseMullion = _parent.FindFalseMullion(_sash);
            if (falseMullion != null)
            {
                float midpointX = falseMullion.Offset.X;
                float distLeftEdge = Math.Abs(midpointX - _sash.Rectangle.Left);
                float distRightEdge = Math.Abs(midpointX - _sash.Rectangle.Right);

                EMountSide sideToCompare = EMountSide.msRight;
                if (distLeftEdge < distRightEdge)
                {
                    // štulp je zleva
                    sideToCompare = EMountSide.msLeft;
                }

                if (falseMullion.MountSide == sideToCompare)
                {
                    // horní křídlo
                    result.FalseMullion1 = true;
                }
                else
                {
                    // dolní křídlo
                    result.FalseMullion2 = true;
                    result.FalseMullionNrArt = falseMullion.Article;
                }
            }
            else
            {
                result.Standard = true;
            }

            return result;
        }

        /// <summary>
        /// Zobrazí standardní dialog WH Oken pro výběr kování.
        /// </summary>
        public void ShowFittingsDialog()
        {
            _sash.OnCommand(Commands.ShowFittingsDialog);
        }

        /// <summary>
        /// Vrací oblast na zadané souřadnici.
        /// </summary>
        /// <param name="pointInAreaX">X-ová souřadnice bodu pro výběr pole.</param>
        /// <param name="pointInAreaY">Y-ová souřadnice bodu pro výběr pole.</param>
        public SashArea GetArea(float pointInAreaX, float pointInAreaY)
        {
            var area = _sash.GetArea(pointInAreaX, pointInAreaY);
            if (area != null)
            {
                return new SashArea(area, this);
            }

            string msg = string.Format(Strings.CannotFindAreaInFrame, this.ID, pointInAreaX, pointInAreaY);
            throw new ModelException(msg);
        }

        /// <summary>
        /// Vrací prázdnou oblast. Pokud existuje více oblastí, zobrazí dialog pro výběr.
        /// </summary>
        public SashArea GetEmptyArea()
        {
            var areas = _sash.Areas.Where(a => a.Child == null).ToList();
            if (areas.Count == 1)
            {
                return new SashArea(areas[0], this);
            }
            else if (areas.Count > 1)
            {
                var area = AreaSelector.SelectArea(this);
                return new SashArea(area, this);
            }

            throw new ModelException(Strings.NoEmptyAreaInSash);
        }

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

        IEnumerable<IArea> IAreaProvider.GetEmptyAreas()
        {
            return _sash.Areas.Where(a => a.Child == null);
        }

        BitmapFrameResult IAreaProvider.GetPositionImage(double scale)
        {
            return _sash.TopObject.GetImageOnly(scale);
        }

        float IAreaProvider.PositionHeight
        {
            get { return _parent.Parent.Height; }
        }

        float IAreaProvider.PositionWidth
        {
            get { return _parent.Parent.Width; }
        }
    }
}
