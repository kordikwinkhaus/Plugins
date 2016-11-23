using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace WindowOffset.Models
{
    internal class XmlAdapter
    {
        private static string[] s_slantKeys = new string[] 
        {
            Xml.TopLeft,
            Xml.TopRight,
            Xml.BottomRight,
            Xml.BottomLeft
        };

        private static string[] s_offsetIds = new string[]
        {
            Xml.Left,
            Xml.TopLeft,
            Xml.Top,
            Xml.TopRight,
            Xml.Right,
            Xml.BottomRight,
            Xml.Bottom,
            Xml.BottomLeft
        };

        private XElement _data;

        internal XmlAdapter(XElement data)
        {
            _data = data;
        }

        internal bool IsWinOffsetSpecified()
        {
            return GetMainElement() != null;
        }

        internal WallHoleData GetCurrentData()
        {
            WallHoleData result = null;

            var mainElement = GetMainElement();
            if (mainElement != null)
            {
                result = new WallHoleData();
                result.MainDimension = DeserializeSlant(mainElement.Element(Xml.Dimensions));

                result.Slants = new SizeF[s_slantKeys.Length];
                for (int i = 0; i < s_slantKeys.Length; i++)
                {
                    var slantElem = mainElement.Elements(Xml.Slant)
                                               .SingleOrDefault(e => e.Attribute(Xml.SlantType).Value == s_slantKeys[i]);
                    if (slantElem != null)
                    {
                        result.Slants[i] = DeserializeSlant(slantElem);
                    }
                }

                result.Offsets = new Dictionary<int, int>();
                var offsetElem = mainElement.Elements(Xml.Offset)
                                            .Single(e => e.Attribute(Xml.OffsetSide).Value == Xml.AllSides);
                result.Offsets.Add(-1, DeserializeOffset(offsetElem));
                for (int i = 0; i < s_offsetIds.Length; i++)
                {
                    offsetElem = mainElement.Elements(Xml.Offset)
                                            .SingleOrDefault(e => e.Attribute(Xml.OffsetSide).Value == s_offsetIds[i]);
                    if (offsetElem != null)
                    {
                        result.Offsets.Add(i, DeserializeOffset(offsetElem));
                    }
                }
            }

            return result;
        }

        internal void SetCurrentData(WallHoleData data)
        {
            var winOffset = GetMainElement(createIfNotExists: true);
            winOffset.RemoveAll();
            winOffset.Add(new XElement(Xml.Dimensions, Serialize(data.MainDimension)));

            for (int i = 0; i < s_slantKeys.Length; i++)
            {
                var slantElem = new XElement(Xml.Slant,
                    Serialize(data.Slants[i]),
                    new XAttribute(Xml.SlantType, s_slantKeys[i]));
                winOffset.Add(slantElem);
            }

            winOffset.Add(new XElement(Xml.Offset, 
                Serialize(data.Offsets[-1]),
                new XAttribute(Xml.OffsetSide, Xml.AllSides)));
            foreach (int id in data.Offsets.Keys)
            {
                if (id == -1) continue;

                var offsetElem =new XElement(Xml.Offset, 
                    Serialize(data.Offsets[id]), 
                    new XAttribute(Xml.OffsetSide, s_offsetIds[id]));
                winOffset.Add(offsetElem);
            }
        }

        private static string Serialize(SizeF size)
        {
            return size.Width.ToString(CultureInfo.InvariantCulture) + "x" + size.Height.ToString(CultureInfo.InvariantCulture);
        }

        private static string Serialize(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        private static SizeF DeserializeSlant(XElement element)
        {
            string data = element.Value;
            string[] parts = data.Split('x');
            float width = float.Parse(parts[0], NumberStyles.Number, CultureInfo.InvariantCulture);
            float height = float.Parse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture);

            return new SizeF(width, height);
        }

        private static int DeserializeOffset(XElement element)
        {
            return int.Parse(element.Value, CultureInfo.InvariantCulture);
        }

        private XElement GetMainElement(bool createIfNotExists = false)
        {
            var mainElement = _data.Element(Xml.WinOffset);
            if (mainElement == null && createIfNotExists)
            {
                mainElement = new XElement(Xml.WinOffset);
                _data.Add(mainElement);
            }
            return mainElement;
        }
    }
}
