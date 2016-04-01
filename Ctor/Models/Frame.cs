using System;
using System.Collections.Generic;
using System.Linq;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt rámu.
    /// </summary>
    public class Frame
    {
        private readonly IFrame _frame;
        private readonly Position _parent;

        internal Frame(IFrame frame, Position parent)
        {
            if (frame == null) throw new ArgumentNullException(nameof(frame));
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _frame = frame;
            _parent = parent;
        }

        /// <summary>
        /// Vrací pozici, ve které je tento rám.
        /// </summary>
        public Position Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// ID typu okna.
        /// </summary>
        public int WindowTypeID
        {
            get { return _frame.FrameType; }
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
        /// Vloží křídlo do prázdného pole, které obsahuje předaný bod.
        /// </summary>
        /// <param name="pointInAreaX">X-ová souřadnice bodu pro výběr pole.</param>
        /// <param name="pointInAreaY">Y-ová souřadnice bodu pro výběr pole.</param>
        public Sash InsertSash(float pointInAreaX, float pointInAreaY)
        {
            foreach (IArea area in _frame.Areas.Where(a => a.Child == null))
            {
                if (area.Rectangle.Contains(pointInAreaX, pointInAreaY))
                {
                    area.AddChild(EProfileType.tSkrz, null);

                    ISash sash = area.FindSash();
                    if (sash != null)
                    {
                        return new Sash(sash, this);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Vloží křídlo do prázdného pole. Pokud rám obsahuje více prázdných polí,
        /// zobrazí dialog pro výběr pole.
        /// </summary>
        public Sash InsertSash()
        {
            var areas = _frame.Areas.Where(a => a.Child == null).ToList();

            if (areas.Count == 1)
            {
                var area = areas[0];
                area.AddChild(EProfileType.tSkrz, null);

                ISash sash = area.FindSash();
                if (sash != null)
                {
                    return new Sash(sash, this);
                }
            }
            else
            {
                // TODO: zobrazit dialog
            }

            return null;
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

            return null;
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
    }
}
