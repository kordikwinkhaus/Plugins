using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt pozice.
    /// Instance třídy je ve skriptu nastavena jako proměnná "pos".
    /// </summary>
    public class Position : IAreaProvider
    {
        private readonly IPosition _position;

        internal Position(IPosition position)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));

            _position = position;
        }

        /// <summary>
        /// Vrací ID pozice.
        /// </summary>
        public int ID
        {
            get { return _position.Pos; }
        }

        /// <summary>
        /// Vrací/nastavuje číslo pozice.
        /// </summary>
        public string Number
        {
            get { return _position.Tag; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException();
                _position.Tag = value;
            }
        }

        public override string ToString()
        {
            return "{ID=" + (this.ID + 1) + ", Number=" + this.Number + "}";
        }

        /// <summary>
        /// Vrací/nastavuje popis pozice.
        /// </summary>
        public string Description
        {
            get { return _position.Description; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException();
                _position.Description = value;
            }
        }

        /// <summary>
        /// Vrací true, pokud má pozice "kreslící plochu".
        /// </summary>
        public bool HasTopObject
        {
            get { return _position.Data != null; }
        }

        /// <summary>
        /// Vrací true, pokud se jedná o pozici okna, dveří nebo posuvných dveří.
        /// </summary>
        public bool IsConstruction
        {
            get { return _position.Type == EPosType.kOkno ||
                         _position.Type == EPosType.kDrzwi ||
                         _position.Type == EPosType.kDrzwiPrzesuwne;
            }
        }

        /// <summary>
        /// Vrací šířku konstrukční plochy pozice.
        /// </summary>
        public float Width
        {
            get
            {
                CheckTopObject();
                return _position.Data.Dimensions.Width;
            }
        }

        /// <summary>
        /// Vrací výšku konstrukční plochy pozice.
        /// </summary>
        public float Height
        {
            get
            {
                CheckTopObject();
                return _position.Data.Dimensions.Height;
            }
        }

        /// <summary>
        /// Vymaže prvky v kreslící ploše.
        /// </summary>
        public void Clear()
        {
            CheckTopObject();

            ITopObject top = _position.Data;
            int count = 0;

            do
            {
                foreach (uint guid in top.FindParts(EProfileType.tOsciez, false).Select(f => f.GUID))
                {
                    var part = _position.Document.FromGUID(guid);
                    if (part != null)
                    {
                        part.OnCommand(Commands.Delete);
                    }
                }

                foreach (IPart part in top.FindParts(EProfileType.tPLacz, true)
                               .Concat(top.FindParts(EProfileType.tNullPLacz, true)))
                {
                    part.OnCommand(Commands.Delete);
                }
            } while (top.Bars.Count() != 0 && count++ < 10);

            top.Invalidate();
        }

        /// <summary>
        /// Zrcadlení pozice.
        /// </summary>
        public void Mirror()
        {
            CheckTopObject();

            IFrameBase frame = _position.Data.GetFrame(EProfileType.tDziura);
            frame.MirrorTransform();
            frame.Update(true);
        }

        /// <summary>
        /// Vrací oblast na zadané souřadnici.
        /// </summary>
        /// <param name="pointInAreaX">X-ová souřadnice bodu pro výběr pole.</param>
        /// <param name="pointInAreaY">Y-ová souřadnice bodu pro výběr pole.</param>
        public PositionArea GetArea(float pointInAreaX, float pointInAreaY)
        {
            CheckTopObject();

            var area = _position.Data.GetArea(pointInAreaX, pointInAreaY);
            if (area != null)
            {
                return new PositionArea(area, this);
            }

            string msg = string.Format(Strings.CannotFindAreaInPosition, pointInAreaX, pointInAreaY);
            throw new ModelException(msg);
        }

        /// <summary>
        /// Vrací prázdnou oblast. Pokud existuje více oblastí, zobrazí dialog pro výběr.
        /// </summary>
        public PositionArea GetEmptyArea()
        {
            CheckTopObject();

            var areas = _position.Data.Areas
                .Where(a => a.Child == null && a.Rectangle.Height != 0 && a.Rectangle.Width != 0)
                .ToList();
            if (areas.Count == 1)
            {
                return new PositionArea(areas[0], this);
            }
            else if (areas.Count > 1)
            {
                var area = AreaSelector.SelectArea(this);
                return new PositionArea(area, this);
            }

            throw new ModelException(Strings.NoEmptyAreaInPosition);
        }

        /// <summary>
        /// Vloží rámy do prázdných polí.
        /// </summary>
        /// <param name="type">ID typu.</param>
        /// <param name="color">ID barvy.</param>
        public void InsertFrames(int type, int color)
        {
            CheckTopObject();

            var parameters = Parameters.ForFrameType(type, color);
            foreach (IArea area in _position.Data.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tOsciez, parameters);
            }
        }

        /// <summary>
        /// Vloží rám do prázdného pole. Pokud pozice obsahuje více prázdných polí, 
        /// zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="type">ID typu.</param>
        /// <param name="color">ID barvy.</param>
        public Frame InsertFrame(int type, int color)
        {
            var area = this.GetEmptyArea();
            return area.InsertFrame(type, color);
        }

        /// <summary>
        /// Vrací rám pro zadaný index.
        /// </summary>
        /// <param name="id">Index rámu.</param>
        public Frame this[int id]
        {
            get { return GetFrame(id); }
        }

        /// <summary>
        /// Vrací rám pro zadaný index.
        /// </summary>
        /// <param name="id">Index rámu.</param>
        public Frame GetFrame(int id)
        {
            CheckTopObject();

            foreach (IFrame frame in _position.Data.FindParts(EProfileType.tOsciez, false))
            {
                int number = frame.GetNumber(EProfileType.tOsciez);
                if (id == number)
                {
                    return new Frame(frame, this);
                }
            }

            throw new ModelException(string.Format(Strings.NoFrameInPosition, id));
        }

        /// <summary>
        /// Vrací všechny rámy v konstrukci.
        /// </summary>
        public IEnumerable<Frame> GetFrames()
        {
            CheckTopObject();

            foreach (IFrame frame in _position.Data.FindParts(EProfileType.tOsciez, false))
            {
                yield return new Frame(frame, this);
            }
        }

        /// <summary>
        /// Vloží zadaný paket do všech prázdných polí (rámy a křídla) v pozici.
        /// </summary>
        /// <param name="nrArt">Číslo výrobku paketu.</param>
        public void InsertGlasspackets(string nrArt)
        {
            var parameters = Parameters.ForGlasspacket(nrArt);
            foreach (var frame in GetFrames())
            {
                frame.InsertGlasspackets(parameters);
            }
        }

        private void CheckTopObject()
        {
            if (_position.Data == null) throw new ModelException(Strings.NoTopObject);
        }

        #region Coupling profile

        /// <summary>
        /// Vloží horizontální spojovací profil do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu spojovacího profilu.</param>
        /// <param name="color">ID barvy.</param>
        public CouplingInsertionResult InsertHorizontalCoupling(string nrArt, int color)
        {
            var area = GetEmptyArea();
            return area.InsertHorizontalCoupling(nrArt, color);
        }

        /// <summary>
        /// Vloží horizontální spojovací profil do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu spojovacího profilu.</param>
        /// <param name="color">ID barvy.</param>
        /// <param name="dimY">Souřadnice spojovacího profilu v ose Y. Pokud je menší než jedna, bere se relativně vzhledem k výšce pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public CouplingInsertionResult InsertHorizontalCoupling(string nrArt, int color, float dimY)
        {
            var area = GetEmptyArea();
            return area.InsertHorizontalCoupling(nrArt, color, dimY);
        }

        /// <summary>
        /// Vloží horizontální spojovací profil do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu spojovacího profilu.</param>
        /// <param name="color">ID barvy.</param>
        public CouplingInsertionResult InsertVerticalCoupling(string nrArt, int color)
        {
            var area = GetEmptyArea();
            return area.InsertVerticalCoupling(nrArt, color);
        }

        /// <summary>
        /// Vloží horizontální spojovací profil do prázdného pole.
        /// Pokud rám obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu spojovacího profilu.</param>
        /// <param name="color">ID barvy.</param>
        /// <param name="dimX">Souřadnice spojovacího profilu v ose X. Pokud je menší než jedna, bere se relativně vzhledem k šířce pole.
        /// Pokud je větší než jedna, bere se jako absolutní souřadnice vůči pozici.</param>
        public CouplingInsertionResult InsertVerticalCoupling(string nrArt, int color, float dimX)
        {
            var area = GetEmptyArea();
            return area.InsertVerticalCoupling(nrArt, color, dimX);
        }

        #endregion

        IEnumerable<IArea> IAreaProvider.GetEmptyAreas()
        {
            return _position.Data.Areas.Where(a => a.Child == null && a.Rectangle.Height != 0 && a.Rectangle.Width != 0);
        }

        BitmapFrameResult IAreaProvider.GetPositionImage(double scale)
        {
            return _position.Data.GetImageOnly(scale);
        }

        float IAreaProvider.PositionHeight
        {
            get { return this.Height; }
        }

        float IAreaProvider.PositionWidth
        {
            get { return this.Width; }
        }
    }
}
