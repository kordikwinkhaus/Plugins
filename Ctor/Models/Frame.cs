using System;
using System.Collections.Generic;
using System.Linq;
using WHOkna;

namespace Ctor.Models
{
    public class Frame
    {
        private readonly IFrameBase _frame;

        internal Frame(IFrameBase frame)
        {
            if (frame == null) throw new ArgumentNullException(nameof(frame));

            _frame = frame;
        }

        public void InsertSashes()
        {
            foreach (IArea area in _frame.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tSkrz, null);
            }
        }

        public IEnumerable<Sash> GetSashes()
        {
            foreach (IFrameBase frame in _frame.FindParts(EProfileType.tSkrz, false))
            {
                yield return new Sash(frame);
            }
        }

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
