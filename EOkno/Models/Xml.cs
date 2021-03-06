﻿namespace EOkno.Models
{
    internal static class Xml
    {
        // hlavní element pluginu
        internal const string EOkno = "EOkno";

        // povrchové úpravy
        internal const string PovrchUprava = "p";
        internal const string UpravaKod = "k";
        internal const string OdstinExterier = "e";
        internal const string OdstinInterier = "i";
        internal const string OdstinNazevExterier = "en";
        internal const string OdstinNazevInterier = "in";

        // komponenty
        internal const string Komponenta = "k";
        internal const string Prace = "p";
        internal const string Material = "m";

        // podle nastavení dokumentu
        internal const string Inherit = "doc";
        internal const string True = "1";
        internal const string False = "0";

        // dokument
        internal const string Sleva = "s";
        internal const string Dph = "d";
    }
}
