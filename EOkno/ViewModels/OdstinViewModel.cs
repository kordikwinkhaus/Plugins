using System;
using System.Diagnostics;

namespace EOkno.ViewModels
{
    [DebuggerDisplay("{Nazev} ({Kod})")]
    public class OdstinViewModel
    {
        internal OdstinViewModel(string kod, string nazev)
        {
            if (string.IsNullOrEmpty(kod)) throw new ArgumentNullException(nameof(kod));
            if (string.IsNullOrEmpty(nazev)) throw new ArgumentNullException(nameof(nazev));

            this.Kod = kod;
            this.Nazev = nazev;
        }

        public string Kod { get; private set; }
        public string Nazev { get; private set; }
    }
}
