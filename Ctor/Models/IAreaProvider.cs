using System.Collections.Generic;
using WHOkna;

namespace Ctor.Models
{
    interface IAreaProvider
    {
        float PositionHeight { get; }

        float PositionWidth { get; }

        BitmapFrameResult GetPositionImage(double scale);

        IEnumerable<IArea> GetEmptyAreas();
    }
}
