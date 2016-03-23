using System;
using System.Collections.Generic;
using System.Linq;
using WHOkna;

namespace Ctor.Models
{
    public class Sash
    {
        private readonly ISash _sash;

        internal Sash(ISash sash)
        {
            if (sash == null) throw new ArgumentNullException(nameof(sash));

            _sash = sash;
        }

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
    }
}
