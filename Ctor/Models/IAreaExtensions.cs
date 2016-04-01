using System.Linq;
using WHOkna;

namespace Ctor.Models
{
    public static class IAreaExtensions
    {
        public static IFrame FindFrame(this IArea area)
        {
            if (area == null) return null;

            IFrameExterior frameExt = area.Child as IFrameExterior;
            if (frameExt != null)
            {
                var parts = frameExt.FindParts(EProfileType.tOsciez, false).ToList();
                if (parts.Count == 1)
                {
                    return parts[0] as IFrame;
                }
            }

            return null;
        }

        public static ISash FindSash(this IArea area)
        {
            if (area == null) return null;

            ISash sash = area.Child as ISash;
            return sash;
        }
    }
}
