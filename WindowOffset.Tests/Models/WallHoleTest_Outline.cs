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

        /*
[TestMethod]
public void Ctor_TopLeftRight_Test()
{
   using (_mocks.Record())
   {
       SetupCommonResults(topLeft: new SizeF(500, 500), topRight: new SizeF(500, 500));
   }

   var target = new WallHole(_data, _topObject);

   Assert.AreEqual(5, target.SideOffsets.Count);
   VerifySides(target, 0, 1, 3, 4, 6);
   VerifyPoints(target,
       new PointF(0, 1000),
       new PointF(0, 500),
       new PointF(500, 0),
       new PointF(1000, 500),
       new PointF(1000, 1000));
   VerifyTopDims(target, 1000, new float[] { 500, 500 });
   VerifyLeftDims(target, 1000, new float[] { 500, 500 });
   VerifyRightDims(target, new float[] { 500, 500 });
   VerifyBottomDims(target);
}

[TestMethod]
public void Ctor_TopLeftTriangle_Test()
{
   using (_mocks.Record())
   {
       SetupCommonResults(topLeft: new SizeF(1000, 1000));
   }

   var target = new WallHole(_data, _topObject);

   Assert.AreEqual(3, target.SideOffsets.Count);
   VerifySides(target, 1, 4, 6);
   VerifyPoints(target,
       new PointF(0, 1000),
       new PointF(1000, 0),
       new PointF(1000, 1000));
   VerifyTopDims(target, 1000);
   VerifyLeftDims(target, 1000);
   VerifyRightDims(target);
   VerifyBottomDims(target);
}

[TestMethod]
public void Ctor_TopRight_Test()
{
   using (_mocks.Record())
   {
       SetupCommonResults(topRight: new SizeF(500, 500));
   }

   var target = new WallHole(_data, _topObject);

   Assert.AreEqual(5, target.SideOffsets.Count);
   VerifySides(target, 0, 2, 3, 4, 6);
   VerifyPoints(target,
       new PointF(0, 1000),
       new PointF(0, 0),
       new PointF(500, 0),
       new PointF(1000, 500),
       new PointF(1000, 1000));
   VerifyTopDims(target, 1000, new float[] { 500, 500 });
   VerifyLeftDims(target, 1000);
   VerifyRightDims(target, new float[] { 500, 500 });
   VerifyBottomDims(target);
}

[TestMethod]
public void Ctor_TopRightTriangle_Test()
{
   using (_mocks.Record())
   {
       SetupCommonResults(topRight: new SizeF(1000, 1000));
   }

   var target = new WallHole(_data, _topObject);

   Assert.AreEqual(3, target.SideOffsets.Count);
   VerifySides(target, 0, 3, 6);
   VerifyPoints(target,
       new PointF(0, 1000),
       new PointF(0, 0),
       new PointF(1000, 1000));
   VerifyTopDims(target, 1000);
   VerifyLeftDims(target, 1000);
   VerifyRightDims(target);
   VerifyBottomDims(target);
}

[TestMethod]
public void Ctor_BottomRight_Test()
{
   using (_mocks.Record())
   {
       SetupCommonResults(bottomRight: new SizeF(500, 500));
   }

   var target = new WallHole(_data, _topObject);

   Assert.AreEqual(5, target.SideOffsets.Count);
   VerifySides(target, 0, 2, 4, 5, 6);
   VerifyPoints(target,
       new PointF(0, 1000),
       new PointF(0, 0),
       new PointF(1000, 0),
       new PointF(1000, 500),
       new PointF(500, 1000));
   VerifyTopDims(target, 1000);
   VerifyLeftDims(target, 1000);
   VerifyRightDims(target, new float[] { 500, 500 });
   VerifyBottomDims(target, new float[] { 500, 500 });
}

[TestMethod]
public void Ctor_BottomRightTriangle_Test()
{
   using (_mocks.Record())
   {
       SetupCommonResults(bottomRight: new SizeF(1000, 1000));
   }

   var target = new WallHole(_data, _topObject);

   Assert.AreEqual(3, target.SideOffsets.Count);
   VerifySides(target, 0, 2, 5);
   VerifyPoints(target,
       new PointF(0, 1000),
       new PointF(0, 0),
       new PointF(1000, 0));
   VerifyTopDims(target, 1000);
   VerifyLeftDims(target, 1000);
   VerifyRightDims(target);
   VerifyBottomDims(target);
}

[TestMethod]
public void Ctor_BottomLeft_Test()
{
   using (_mocks.Record())
   {
       SetupCommonResults(bottomLeft: new SizeF(300, 400));
   }

   var target = new WallHole(_data, _topObject);

   Assert.AreEqual(5, target.SideOffsets.Count);
   VerifySides(target, 0, 2, 4, 6, 7);
   VerifyPoints(target,
       new PointF(0, 600),
       new PointF(0, 0),
       new PointF(1000, 0),
       new PointF(1000, 1000),
       new PointF(300, 1000));
   VerifyTopDims(target, 1000);
   VerifyLeftDims(target, 1000, new float[] { 600, 400 });
   VerifyRightDims(target);
   VerifyBottomDims(target, new float[] { 300, 700 });
}

private void VerifySides(WallHole target, params int[] sides)
{
   foreach (var side in sides)
   {
       Assert.AreEqual(1, target.SideOffsets.Count(i => i.Side == side));
   }
}

private void VerifyPoints(WallHole target, params PointF[] points)
{
   Assert.AreEqual(target.SideOffsets.Count, points.Length);
   for (int i = 0; i < points.Length; i++)
   {
       Assert.AreEqual(points[i], target.SideOffsets[i].Start);
   }

   for (int i = 0; i < target.SideOffsets.Count - 1; i++)
   {
       Assert.AreEqual(target.SideOffsets[i + 1].Start, target.SideOffsets[i].End);
   }
   Assert.AreEqual(target.SideOffsets.First().Start, target.SideOffsets.Last().End);
}

private void VerifyTopDims(WallHole target, float mainDim, float[] subDims = null)
{
   VerifyMainSideDims(target.TopDims, mainDim, subDims);
}

private void VerifyLeftDims(WallHole target, float mainDim, float[] subDims = null)
{
   VerifyMainSideDims(target.LeftDims, mainDim, subDims);
}

private static void VerifyMainSideDims(IList<DimensionLayer> layers, float mainDim, float[] subDims)
{
   int layersCount = (subDims != null && subDims.Length != 0) ? 2 : 1;

   Assert.AreEqual(layersCount, layers.Count);
   Assert.AreEqual(1, layers[0].Count);
   Assert.AreEqual(mainDim, layers[0][0].Value);
   Assert.AreEqual(0, layers[0][0].From);

   if (layersCount == 2)
   {
       var subLayer = layers[1];
       Assert.AreEqual(subDims.Length, subLayer.Count);
       float sumFrom = 0;
       for (int i = 0; i < subDims.Length; i++)
       {
           Assert.AreEqual(subDims[i], subLayer[i].Value);
           Assert.AreEqual(sumFrom, subLayer[i].From);
           sumFrom += subLayer[i].Value;
       }
   }
}

private void VerifyRightDims(WallHole target, float[] subDims = null)
{
   VerifySubSideDims(target.RightDims, subDims);
}

private void VerifyBottomDims(WallHole target, float[] subDims = null)
{
   VerifySubSideDims(target.BottomDims, subDims);
}

private void VerifySubSideDims(IList<DimensionLayer> layers, float[] subDims)
{
   int layersCount = (subDims != null && subDims.Length != 0) ? 1 : 0;

   Assert.AreEqual(layersCount, layers.Count);
   if (layersCount == 1)
   {
       var layer = layers[0];
       Assert.AreEqual(subDims.Length, layer.Count);
   }
}
*/
        const float DELTA = 0.01f;

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
