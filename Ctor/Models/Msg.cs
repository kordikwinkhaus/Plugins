using System.Windows;

namespace Ctor.Models
{
    /// <summary>
    /// Pomocná třída pro zobrazování zpráv.
    /// Instance třídy nastavena jako proměnná "msg".
    /// </summary>
    public class Msg
    {
        private const string CAPTION = "Ctor plugin";

        /// <summary>
        /// Zobrazí okno se zprávou jako informaci.
        /// </summary>
        /// <param name="msg">Zpráva.</param>
        public void Info(string msg)
        {
            MessageBox.Show(msg, CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Zobrazí okno se zprávou jako výstrahu.
        /// </summary>
        /// <param name="msg">Zpráva.</param>
        public void Alert(string msg)
        {
            MessageBox.Show(msg, CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Zobrazí okno se zprávou jako chybu.
        /// </summary>
        /// <param name="msg">Zpráva.</param>
        public void Error(string msg)
        {
            MessageBox.Show(msg, CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Zobrazí okno se zprávou jako chybu a ukončí provádění skriptu.
        /// </summary>
        /// <param name="msg">Zpráva.</param>
        public void Fail(string msg)
        {
            this.Error(msg);
            throw new IronPython.Runtime.Exceptions.SystemExitException();
        }
    }
}
