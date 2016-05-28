using System.Collections.Generic;
using System.Linq;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    public abstract class FrameBase : Part
    {
        private readonly IFrameBase _frameBase;

        internal FrameBase(IFrameBase frameBase)
            : base(frameBase)
        {
            _frameBase = frameBase;
        }

        /// <summary>
        /// Vloží zadaný paket do prázdných polí.
        /// </summary>
        /// <param name="nrArt">Číslo výrobku paketu.</param>
        public void InsertGlasspackets(string nrArt)
        {
            InsertGlasspackets(Parameters.ForGlasspacket(nrArt));
        }

        internal virtual void InsertGlasspackets(Dictionary<string, object> parameters)
        {
            foreach (IArea area in _frameBase.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tSzyba, parameters);
            }
        }

        /// <summary>
        /// Vloží sklo do prázdného pole. Pokud prvek
        /// obsahuje více prázdných polí, zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="nrArt">Číslo výrobku paketu.</param>
        public Glasspacket InsertGlasspacket(string nrArt)
        {
            var area = this.GetEmptyAreaForGlasspacket();
            return area.InsertGlasspacket(nrArt);
        }

        /// <summary>
        /// Vrací všechny pakety.
        /// </summary>
        /// <returns>Kolekce paketů.</returns>
        public IEnumerable<Glasspacket> GetGlasspackets()
        {
            foreach (IGlazing glazing in _frameBase.FindParts(EProfileType.tSzyba, false))
            {
                yield return CreateGlasspacket(glazing);
            }
        }

        /// <summary>
        /// Vrací paket skla pro zadaný index.
        /// </summary>
        /// <param name="id">Index paketu.</param>
        public Glasspacket GetGlasspacket(int id)
        {
            foreach (IGlazing glazing in _frameBase.FindParts(EProfileType.tSzyba, false))
            {
                int number = glazing.GetNumber(EProfileType.tSzyba);
                if (id == number)
                {
                    return CreateGlasspacket(glazing);
                }
            }

            string msg = string.Format(Strings.NoGlasspacket, id);
            throw new ModelException(msg);
        }

        protected abstract Glasspacket CreateGlasspacket(IGlazing glazing);

        protected abstract FrameAreaBase GetEmptyAreaForGlasspacket();
    }
}
