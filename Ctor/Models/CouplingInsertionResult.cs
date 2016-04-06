namespace Ctor.Models
{
    /// <summary>
    /// Výsledek vložení spojovacího profilu.
    /// </summary>
    public class CouplingInsertionResult
    {
        internal CouplingInsertionResult()
        {
        }

        /// <summary>
        /// Levá nebo horní oblast po vložení spojovacího profilu.
        /// </summary>
        public PositionArea Area1 { get; internal set; }

        /// <summary>
        /// Pravá nebo dolní oblast po vložení spojovacího profilu.
        /// </summary>
        public PositionArea Area2 { get; internal set; }

        /// <summary>
        /// Spojovací profil.
        /// </summary>
        public Bar Coupling { get; internal set; }
    }
}