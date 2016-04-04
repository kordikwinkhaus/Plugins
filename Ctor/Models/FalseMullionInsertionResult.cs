namespace Ctor.Models
{
    /// <summary>
    /// Výsledek vložení štulpu.
    /// </summary>
    public class FalseMullionInsertionResult
    {
        internal FalseMullionInsertionResult()
        {
        }

        /// <summary>
        /// Levé křídlo.
        /// </summary>
        public Sash LeftSash { get; internal set; }

        /// <summary>
        /// Pravé křídlo.
        /// </summary>
        public Sash RightSash { get; internal set; }

        /// <summary>
        /// Štulp.
        /// </summary>
        public FalseMullion FalseMullion { get; internal set; }
    }
}
