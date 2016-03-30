namespace Ctor.Models
{
    public interface IContext
    {
        /// <summary>
        /// ID typu okna.
        /// </summary>
        int WindowsType { get; }

        /// <summary>
        /// ID barvy typu.
        /// </summary>
        int WindowsColor { get; }
    }
}
