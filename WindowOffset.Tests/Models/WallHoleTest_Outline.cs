using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WHOkna;
using WindowOffset.Models;

namespace WindowOffset.Tests.Models
{
    [TestClass]
    public class WallHoleTest_Outline
    {
        const float DELTA = 0.01f;
        MockRepository _mocks;
        ITopObject _topObject;
        RectangleF _dimensions;
        XElement _data;

        [TestInitialize]
        public void Init()
        {
            _mocks = new MockRepository();
            _topObject = _mocks.StrictMock<ITopObject>();
            _dimensions = new RectangleF(0, 0, 1000, 1000);
            _data = new XElement("TODO");
        }

        [TestMethod]
        public void GetWindowOutline_TopLeftTriangle_SameOffset_Test()
        {
            using (_mocks.Record())
            {
                SetupCommonResults(topLeft: new SizeF(1000, 1000));
            }
            var target = new WallHole(_data, _topObject);
            target.MainOffset.Offset = 50;

            var result = target.GetWindowOutline();

            float a = 829.289f;
            VerifyOutline(result, a, a, topLeft: new SizeF(a, a));
        }

        [TestMethod]
        public void GetWindowOutline_TopRight_DiffOffset_RemovePart_Test()
        {
            using (_mocks.Record())
            {
                SetupCommonResults(topRight: new SizeF(940, 1000));
            }
            var target = new WallHole(_data, _topObject);
            target.MainOffset.Offset = 50;
            target.SideOffsets.Single(s => s.Side == 3).Offset = 100;
            target.SideOffsets.Single(s => s.Side == 6).Offset = 20;

            var result = target.GetWindowOutline();
            VerifyOutline(result, 793.956f, 844.633f, topRight: new SizeF(793.956f, 844.633f));
        }

        [TestMethod]
        public void GetWindowOutline_TopRight_DiffOffset_Remove2Parts_Test()
        {
            using (_mocks.Record())
            {
                SetupCommonResults(topRight: new SizeF(500, 900), bottomLeft: new SizeF(850, 400));
            }
            var target = new WallHole(_data, _topObject);
            target.MainOffset.Offset = 50;
            target.SideOffsets.Single(s => s.Side == 3).Offset = 100;
            target.SideOffsets.Single(s => s.Side == 7).Offset = 150;

            var result = target.GetWindowOutline();
            VerifyOutline(result, 798.728f, 783.622f, topRight: new SizeF(435.346f, 783.622f), bottomLeft: new SizeF(798.728f, 375.872f));
        }

        [TestMethod]
        public void GetWallHoleData_Test()
        {
            var tl = new SizeF(10, 20);
            var tr = new SizeF(30, 40);
            var br = new SizeF(50, 60);
            var bl = new SizeF(70, 80);

            using (_mocks.Record())
            {
                SetupCommonResults(topLeft: tl, topRight: tr, bottomRight: br, bottomLeft: bl);
            }
            var target = new WallHole(_data, _topObject);
            target.MainOffset.Offset = 50;
            target.SideOffsets.Single(s => s.Side == 3).Offset = 100;
            target.SideOffsets.Single(s => s.Side == 7).Offset = 150;

            var wallHoleData = target.GetWallHoleData();

            VerifySize(_dimensions.Size, wallHoleData.MainDimension);
            VerifySize(tl, wallHoleData.Slants[0]);
            VerifySize(tr, wallHoleData.Slants[1]);
            VerifySize(br, wallHoleData.Slants[2]);
            VerifySize(bl, wallHoleData.Slants[3]);

            Assert.AreEqual(3, wallHoleData.Offsets.Count);
            Assert.AreEqual(50, wallHoleData.Offsets[-1]);
            Assert.AreEqual(100, wallHoleData.Offsets[3]);
            Assert.AreEqual(150, wallHoleData.Offsets[7]);
        }

        private void VerifyOutline(WindowOutline result, float width, float height, 
            SizeF topLeft = new SizeF(), SizeF topRight = new SizeF(), SizeF bottomLeft = new SizeF(), SizeF bottomRight = new SizeF())
        {
            Assert.AreEqual(width, result.Size.Width, DELTA);
            Assert.AreEqual(height, result.Size.Height, DELTA);
            VerifySize(topLeft, result.TopLeft);
            VerifySize(topRight, result.TopRight);
            VerifySize(bottomLeft, result.BottomLeft);
            VerifySize(bottomRight, result.BottomRight);
        }

        private void VerifySize(SizeF expected, SizeF actual)
        {
            Assert.AreEqual(expected.Width, actual.Width, DELTA);
            Assert.AreEqual(expected.Height, actual.Height, DELTA);
        }

        private void SetupCommonResults(SizeF topLeft = new SizeF(), SizeF topRight = new SizeF(),
            SizeF bottomLeft = new SizeF(), SizeF bottomRight = new SizeF())
        {
            SetupResult.For(_topObject.Dimensions).Return(_dimensions);
            SetupResult.For(_topObject.get_Slants(0)).Return(topLeft);
            SetupResult.For(_topObject.get_Slants(1)).Return(topRight);
            SetupResult.For(_topObject.get_Slants(2)).Return(bottomRight);
            SetupResult.For(_topObject.get_Slants(3)).Return(bottomLeft);
        }
    }
}
