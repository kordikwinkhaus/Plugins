namespace Ctor.Models
{
    public interface IContext
    {
        /// <summary>
        /// ID typu okna.
        /// </summary>
        int Type { get; }

        /// <summary>
        /// ID barvy typu.
        /// </summary>
        int Color { get; }
    }
}
