using System;
using System.Xml.Linq;

namespace EOkno
{
    public static class XElementExtensions
    {
        public static bool GetDefault(this XElement element)
        {
            var attr = element.Attribute("default");
            if (attr != null)
            {
                return 0 == string.Compare(attr.Value, "true", StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
    }
}
