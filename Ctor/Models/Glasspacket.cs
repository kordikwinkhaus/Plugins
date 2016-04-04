using System;
using System.Collections;
using System.Collections.Generic;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt zasklení.
    /// </summary>
    public class Glasspacket : Part
    {
        private readonly IGlazing _glazing;
        private readonly Sash _parent;

        internal Glasspacket(IGlazing glazing, Sash parent)
            : base(glazing)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _glazing = glazing;
            _parent = parent;
        }

        internal IGlazing Data
        {
            get { return _glazing; }
        }

        // TODO: společná třída pro rám a křídlo
        public Sash Parent { get; set; }

        /// <summary>
        /// Vrací ID skla (pořadová čísla od jedné).
        /// </summary>
        public int ID
        {
            get { return _glazing.GetNumber(EProfileType.tSzyba); }
        }
    }
}
