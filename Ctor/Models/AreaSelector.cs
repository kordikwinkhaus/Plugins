using System;
using WHOkna;

namespace Ctor.Models
{
    internal static class AreaSelector
    {
        internal static Func<IAreaProvider, IArea> SelectArea { get; set; }
    }
}
