using System;

namespace Ctor.Models
{
    /// <summary>
    /// Třída výjimky pro chyby v modelu.
    /// </summary>
    /// <remarks>
    /// Tato výjimka je při vykonání skriptu obsloužena speciálně - nepovažuje
    /// se za neočekávaný chybový stav, ale slouží k předčasnému ukončení skriptu (např. z důvodu chybějícího vstupu); 
    /// a zpráva výjimky je zobrazena v okně se zprávou po dokončení skriptu.
    /// </remarks>
    public class ModelException : Exception
    {
        internal ModelException(string message) 
            : base(message)
        {
        }
    }
}
