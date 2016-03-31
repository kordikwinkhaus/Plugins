using System.Collections.Generic;

namespace Ctor.Models
{
    /// <summary>
    /// Rozhraní databáze.
    /// Instance třídy, která implementuje toto rozrahní, je ve skriptu nastavena jako proměnná "db".
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Vrací typ okna.
        /// </summary>
        /// <param name="id">ID typu okna.</param>
        WindowType GetWindowType(int id);

        /// <summary>
        /// Ověří, zda-li je daný profil virtuální.
        /// </summary>
        /// <param name="nrArt">Číslo artiklu profilu.</param>
        /// <param name="table">Název tabulky, která obsahuje daný profil.</param>
        bool IsVirtual(string nrArt, string table);

        /// <summary>
        /// Vrací společné barvy profilu pro zadaná čísla artiklů.
        /// </summary>
        /// <param name="articles">Čísla artiklů.</param>
        /// <returns>Vrací indexy společných barev.</returns>
        List<int> GetCommonColors(IList<string> articles);
    }
}