using System;
using System.Xml.Linq;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Základní objekt prvku okna.
    /// </summary>
    public class Part
    {
        private readonly IPart _part;

        internal Part(IPart part)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));

            _part = part;
        }

        /// <summary>
        /// Vrací šířku prvku.
        /// </summary>
        public virtual float Width
        {
            get { return _part.Rectangle.Width; }
        }

        /// <summary>
        /// Vrací výšku oblasti.
        /// </summary>
        public virtual float Height
        {
            get { return _part.Rectangle.Height; }
        }

        /// <summary>
        /// Vrací souřadnici levé hrany.
        /// </summary>
        public virtual float Left
        {
            get { return _part.Rectangle.Left; }
        }

        /// <summary>
        /// Vrací souřadnici pravé hrany.
        /// </summary>
        public virtual float Right
        {
            get { return _part.Rectangle.Right; }
        }

        /// <summary>
        /// Vrací souřadinici horní hrany.
        /// </summary>
        public virtual float Top
        {
            get { return _part.Rectangle.Top; }
        }

        /// <summary>
        /// Vrací souřadnici spodní hrany.
        /// </summary>
        public virtual float Bottom
        {
            get { return _part.Rectangle.Bottom; }
        }

        /// <summary>
        /// Vrací XML element s uživatelskými daty.
        /// </summary>
        public XElement UserData
        {
            get { return _part.ExtendedProperties; }
        }
    }
}
