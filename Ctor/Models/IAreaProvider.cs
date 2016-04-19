using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WHOkna;

namespace Ctor.Models
{
    interface IAreaProvider
    {
        IEnumerable<IArea> GetEmptyAreas();

        IEnumerable<IArea> GetUsedAreas();
    }
}
