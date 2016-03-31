namespace Ctor.Models
{
    /// <summary>
    /// Kontext vkládání.
    /// Instance třídy, která implementuje toto rozrahní, je ve skriptu nastavena jako proměnná "ctx".
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// ID vybraného typu okna.
        /// Pokud není vybráno, implementace by měla vyhodit výjimku <see cref="ModelException"/>.
        /// </summary>
        int WindowsType { get; }

        /// <summary>
        /// ID vybrané barvy.
        /// Pokud není vybráno, implementace by měla vyhodit výjimku <see cref="ModelException"/>.
        /// </summary>
        int WindowsColor { get; }

        /// <summary>
        /// Číslo paketu (buďto výchozího z typu nebo vybraného).
        /// Pokud není vybráno, implementace by měla vyhodit výjimku <see cref="ModelException"/>.
        /// </summary>
        string Glasspacket { get; }
    }
}
