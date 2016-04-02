using Ctor.Resources;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt plochy v pozici.
    /// </summary>
    public class PositionArea : Area
    {
        private readonly Position _parent;

        internal PositionArea(IArea area, Position parent)
            : base(area)
        {
            _parent = parent;
        }

        /// <summary>
        /// Vloží rám do tohoto pole.
        /// </summary>
        /// <param name="type">ID typu.</param>
        /// <param name="color">ID barvy.</param>
        public Frame InsertFrame(int type, int color)
        {
            CheckInvalidation();

            var parameters = Parameters.ForFrameType(type, color);
            _area.AddChild(EProfileType.tOsciez, parameters);

            IFrame frame = _area.FindFrame();
            if (frame != null)
            {
                return new Frame(frame, _parent);
            }

            throw new ModelException(Strings.CannotInsertFrame);
        }
    }
}
