using System.Collections.Generic;
using Okna.Data;

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
        /// Vrací skupinu kování.
        /// </summary>
        /// <param name="id">ID skupiny kování.</param>
        FittingsGroup GetFittingsGroup(int id);

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

        /// <summary>
        /// Provede databázový dotaz.
        /// </summary>
        /// <param name="cmd">Dotaz.</param>
        /// <returns>Seznam načtených záznamů.</returns>
        IList<DynamicDictionary> ExecuteQuery(DatabaseCommand cmd);

        /// <summary>
        /// Vyhledá kování v této skupině a vrátí první nalezený typ.
        /// </summary>
        /// <param name="fittingsGroupID">ID skupiny kování.</param>
        /// <param name="args">Parametry pro vyhledání kování.</param>
        int FindFittingType(int fittingsGroupID, FittingsFindArgs args);
    }
}