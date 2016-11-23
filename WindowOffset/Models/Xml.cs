namespace WindowOffset.Models
{
    internal static class Xml
    {
        // hlavní element pluginu
        internal const string WinOffset = "WinOffset";

        // element hlavních rozměrů
        internal const string Dimensions = "dims";

        // element šikminy
        internal const string Slant = "s";
        // atribut strany šikminy
        internal const string SlantType = "t";

        // element offsetu
        internal const string Offset = "o";
        // atribut strany offsetu
        internal const string OffsetSide = "s";

        // strany šikminy/offsetu
        internal const string TopLeft = "tl";
        internal const string TopRight = "tr";
        internal const string BottomLeft = "bl";
        internal const string BottomRight = "br";
        internal const string Left = "l";
        internal const string Top = "t";
        internal const string Right = "r";
        internal const string Bottom = "b";
        internal const string AllSides = "all";
    }
}
