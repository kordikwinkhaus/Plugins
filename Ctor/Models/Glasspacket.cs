using System;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt zasklení.
    /// </summary>
    public class Glasspacket : Part
    {
        private readonly IGlazing _glazing;
        private readonly FrameBase _parent;

        internal Glasspacket(IGlazing glazing, Frame parent)
            : base(glazing)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _glazing = glazing;
            _parent = parent;

            this.ParentFrame = parent;
        }

        internal Glasspacket(IGlazing glazing, Sash parent)
            : base(glazing)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _glazing = glazing;
            _parent = parent;

            this.ParentSash = parent;
            this.ParentFrame = parent.Parent;
        }

        internal IGlazing Data
        {
            get { return _glazing; }
        }

        /// <summary>
        /// Rodičovské křídlo.
        /// Vrací null pokud je sklo v rámu.
        /// </summary>
        public Sash ParentSash { get; private set; }

        /// <summary>
        /// Rodičovský rám.
        /// Je nastaveno i v případě, že sklo je vsazeno do křídla.
        /// </summary>
        public Frame ParentFrame { get; private set; }

        /// <summary>
        /// Vrací ID skla (pořadová čísla od jedné).
        /// </summary>
        public int ID
        {
            get { return _glazing.GetNumber(EProfileType.tSzyba); }
        }
    }
}
