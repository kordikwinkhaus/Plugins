using System.Text;
using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Základní objekt plochy.
    /// </summary>
    public abstract class Area : Part
    {
        private bool _isInvalid;
        protected readonly IArea _area;

        internal Area(IArea area)
            : base(area)
        {
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
        public override float Width
        {
            get
            {
                CheckInvalidation();
                return base.Width;
            }
        }

        /// <summary>
        /// Vrací výšku oblasti.
        /// </summary>
        public override float Height
        {
            get
            {
                CheckInvalidation();
                return base.Height;
            }
        }

        /// <summary>
        /// Vrací souřadnici levé hrany.
        /// </summary>
        public override float Left
        {
            get
            {
                CheckInvalidation();
                return base.Left;
            }
        }

        /// <summary>
        /// Vrací souřadnici pravé hrany.
        /// </summary>
        public override float Right
        {
            get
            {
                CheckInvalidation();
                return base.Right;
            }
        }

        /// <summary>
        /// Vrací souřadinici horní hrany.
        /// </summary>
        public override float Top
        {
            get
            {
                CheckInvalidation();
                return base.Top;
            }
        }

        /// <summary>
        /// Vrací souřadnici spodní hrany.
        /// </summary>
        public override float Bottom
        {
            get
            {
                CheckInvalidation();
                return base.Bottom;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{Width=")
              .Append(this.Width)
              .Append(", Height=")
              .Append(this.Height)
              .Append("}");

            return sb.ToString();
        }
    }
}
