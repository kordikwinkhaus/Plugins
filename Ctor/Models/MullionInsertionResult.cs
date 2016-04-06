namespace Ctor.Models
{
    /// <summary>
    /// Výsledek vložení sloupku.
    /// </summary>
    public class MullionInsertionResult<TArea> where TArea : Area
    {
        internal MullionInsertionResult()
        {
        }

        /// <summary>
        /// Levá nebo horní oblast po vložení sloupku.
        /// </summary>
        public TArea Area1 { get; internal set; }

        /// <summary>
        /// Pravá nebo dolní oblast po vložení sloupku.
        /// </summary>
        public TArea Area2 { get; internal set; }

        /// <summary>
        /// Vložený sloupek.
        /// </summary>
        public Bar Mullion { get; internal set; }
    }
}
