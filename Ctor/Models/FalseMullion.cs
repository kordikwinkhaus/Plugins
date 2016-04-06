using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt štulpu.
    /// </summary>
    public class FalseMullion : Bar
    {
        private readonly IFalseMullion _falseMullion;

        internal FalseMullion(IFalseMullion falseMullion)
            : base(falseMullion)
        {
            _falseMullion = falseMullion;
        }

        /// <summary>
        /// Strana montáže štulpu.
        /// </summary>
        public bool IsLeftSide
        {
            get { return _falseMullion.MountSide == EMountSide.msLeft; }
            set { _falseMullion.MountSide = (value) ? EMountSide.msLeft : EMountSide.msRight; }
        }
    }
}
