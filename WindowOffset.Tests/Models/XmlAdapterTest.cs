using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowOffset.Models;

namespace WindowOffset.Tests.Models
{
    [TestClass]
    public class XmlAdapterTest
    {
        const float DELTA = 0.01f;

        [TestMethod]
        public void Ctor_EmptyData_Test()
        {
            XElement data = new XElement("UserData");
            var target = new XmlAdapter(data);

            Assert.IsFalse(target.IsWinOffsetSpecified());
        }

        [TestMethod]
        public void Ctor_SpecifiedData_Test()
        {
            string xml = @"<UserData>
  <WinOffset/>
</UserData>";

            XElement data = XElement.Parse(xml);
            var target = new XmlAdapter(data);

            Assert.IsTrue(target.IsWinOffsetSpecified());
        }

        [TestMethod]
        public void SetCurrentData_GetCurrentData_Test()
        {
            WallHoleData expectedData = GetSourceData();
            XElement data = new XElement("UserData");
            var target = new XmlAdapter(data);

            target.SetCurrentData(expectedData);

            var target2 = new XmlAdapter(data);

            var actualData = target2.GetCurrentData();

            Verify(expectedData, actualData);
        }

        private void Verify(WallHoleData expectedData, WallHoleData actualData)
        {
            VerifySize(expectedData.MainDimension, actualData.MainDimension);
            for (int i = 0; i < 4; i++)
            {
                VerifySize(expectedData.Slants[i], actualData.Slants[i]);
            }

            Assert.AreEqual(expectedData.Offsets.Count, actualData.Offsets.Count);
            foreach (var key in expectedData.Offsets.Keys)
            {
                Assert.IsTrue(actualData.Offsets.ContainsKey(key));
                Assert.AreEqual(expectedData.Offsets[key], actualData.Offsets[key]);
            }
        }

        private void VerifySize(SizeF expected, SizeF actual)
        {
            Assert.AreEqual(expected.Width, actual.Width, DELTA);
            Assert.AreEqual(expected.Height, actual.Height, DELTA);
        }

        private WallHoleData GetSourceData()
        {
            return new WallHoleData
            {
                MainDimension = new SizeF(2000, 1000),
                Slants = new SizeF[]
                {
                    new SizeF(10, 20),
                    new SizeF(30, 40),
                    new SizeF(50, 60),
                    new SizeF(70, 80)
                },
                Offsets = new Dictionary<int, int>
                {
                    { -1, 30 },
                    { 2, 50 },
                    { 5, 10 }
                }
            };
        }
    }
}
