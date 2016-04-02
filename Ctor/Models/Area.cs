using System;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Základní objekt plochy.
    /// </summary>
    public abstract class Area
    {
        protected readonly IArea _area;

        internal Area(IArea area)
        {
            if (area == null) throw new ArgumentNullException(nameof(area));

            _area = area;
        }

        /// <summary>
        /// Vrací true, pokud je oblast prázdná.
        /// </summary>
        public bool IsEmpty
        {
            get { return _area.Child == null; }
        }


    }
}
