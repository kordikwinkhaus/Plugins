using System.Drawing;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt štulpu.
    /// </summary>
    public class FalseMullion : Part
    {
        private readonly IFalseMullion _falseMullion;

        internal FalseMullion(IFalseMullion falseMullion)
            : base(falseMullion)
        {
            _falseMullion = falseMullion;
        }

        /// <summary>
        /// Bod vložení štulpu.
        /// </summary>
        public PointF InsertionPoint
        {
            get { return _falseMullion.Offset; }
            set { _falseMullion.Offset = value; }
        }
    }
}
