using System;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Základní objekt plochy.
    /// </summary>
    public abstract class Area
    {
        private bool _isInvalid;
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
            get
            {
                CheckInvalidation();
                return _area.Child == null;
            }
        }

        /// <summary>
        /// Vrací true, pokud byla tato oblast zneplatněna (po vložení sloupku, štulpu).
        /// </summary>
        public bool IsInvalid
        {
            get { return _isInvalid; }
        }

        /// <summary>
        /// Ověří, zda-li oblast není zneplatněna.
        /// </summary>
        protected void CheckInvalidation()
        {
            if (_isInvalid) throw new ModelException(Strings.InvalidArea);
        }

        /// <summary>
        /// Zneplatní oblast.
        /// </summary>
        protected void Invalidate()
        {
            _isInvalid = true;
        }

        /// <summary>
        /// Vrací šířku oblasti.
        /// </summary>
        public float Width
        {
            get
            {
                CheckInvalidation();
                return _area.Rectangle.Width;
            }
        }

        /// <summary>
        /// Vrací výšku oblasti.
        /// </summary>
        public float Height
        {
            get
            {
                CheckInvalidation();
                return _area.Rectangle.Height;
            }
        }
    }
}
