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

        internal Frame(IFrame frame)
        {
            if (frame == null) throw new ArgumentNullException(nameof(frame));

            _frame = frame;
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
        /// Vrací všechna křídla.
        /// </summary>
        /// <returns>Kolekce křídel.</returns>
        public IEnumerable<Sash> GetSashes()
        {
            foreach (ISash sash in _frame.FindParts(EProfileType.tSkrz, false))
            {
                yield return new Sash(sash);
            }
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
    }
}
