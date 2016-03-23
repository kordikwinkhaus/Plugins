using System.Xml.Linq;

namespace Okna.Plugins
{
    public static class XmlExtensions
    {
        public static bool HasAttribute(this XElement element, string attrName, string attrValue)
        {
            if (element != null)
            {
                var attr = element.Attribute(attrName);
                if (attr != null)
                {
                    return attr.Value == attrValue;
                }
            }

            return false;
        }

        public static string GetAttrValue(this XElement element, string attrName, string defaultValue)
        {
            var attr = element.Attribute(attrName);
            if (attr != null)
            {
                return attr.Value;
            }
            return defaultValue;
        }
    }
}
