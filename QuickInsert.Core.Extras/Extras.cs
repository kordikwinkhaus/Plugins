using System.Drawing;
using QuickInsert.Models;
using WHOkna;

namespace QuickInsert.Core
{
    public static class Extras
    {
        public static void VlozitOkop(Sash sash, string nrArt)
        {
            int colorID = sash.Color;

            var okopParams = Parameters.ForMullion(nrArt, colorID);
            var emptyArea = sash.GetEmptyArea();
            var insertionPoint = new PointF((emptyArea.Left + emptyArea.Right) / 2, (emptyArea.Top + emptyArea.Bottom) / 2);
            var area = (IArea)(emptyArea.Data);
            var result = area.AddBar(EProfileType.tNakladka, EDir.dLeft, insertionPoint, okopParams);
            area.TopObject.Update("Okop se nepodařilo vložit.");

            if (result != null)
            {
                var bar = (IBar)result[0];
                bar.SlidedToEdge = EDir.dBottom;
                bar.TopObject.Update("Okop nelze zarovnat.");
            }
            else
            {
                throw new ModelException("Okop se nepodařilo vložit.");
            }
        }
    }
}
