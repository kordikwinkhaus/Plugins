using WHOkna;

namespace Ctor.Models
{
    public static class IFrameBaseExtensions
    {
        public static IArea GetArea(this IFrameBase framebase, float pointInAreaX, float pointInAreaY)
        {
            if (framebase != null)
            {
                foreach (var area in framebase.Areas)
                {
                    if (area.Rectangle.Contains(pointInAreaX, pointInAreaY))
                    {
                        return area;
                    }
                }
            }

            return null;
        }
    }
}
