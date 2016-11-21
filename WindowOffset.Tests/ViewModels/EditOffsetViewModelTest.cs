using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WHOkna;
using WindowOffset.ViewModels;

namespace WindowOffset.Tests.ViewModels
{
    [TestClass]
    public class EditOffsetViewModelTest
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

        //[TestMethod]
        //public void Ctor_TopLeft_Test()
        //{
        //    using (_mocks.Record())
        //    {
        //        SetupCommonResults(topLeft: new SizeF(500, 500));
        //    }

        //    var target = new EditOffsetViewModel(_data, _topObject);

        //    Assert.AreEqual(6, target.CanvasItems.Count);
        //    VerifySides(target, 0, 1, 2, 4, 6);
        //    Assert.AreEqual(5, target.WindowPoints.Count);
        //}

        //[TestMethod]
        //public void Ctor_TopLeftRight_Test()
        //{
        //    using (_mocks.Record())
        //    {
        //        SetupCommonResults(topLeft: new SizeF(500, 500), topRight: new SizeF(500, 500));
        //    }

        //    var target = new EditOffsetViewModel(_data, _topObject);

        //    Assert.AreEqual(6, target.CanvasItems.Count);
        //    VerifySides(target, 0, 1, 3, 4, 6);
        //    Assert.AreEqual(5, target.WindowPoints.Count);
        //}

        //[TestMethod]
        //public void Ctor_TopLeftTriangle_Test()
        //{
        //    using (_mocks.Record())
        //    {
        //        SetupCommonResults(topLeft: new SizeF(1000, 1000));
        //    }

        //    var target = new EditOffsetViewModel(_data, _topObject);

        //    Assert.AreEqual(4, target.CanvasItems.Count);
        //    VerifySides(target, 1, 4, 6);
        //    Assert.AreEqual(3, target.WindowPoints.Count);
        //}

        //[TestMethod]
        //public void Ctor_TopRight_Test()
        //{
        //    using (_mocks.Record())
        //    {
        //        SetupCommonResults(topRight: new SizeF(500, 500));
        //    }

        //    var target = new EditOffsetViewModel(_data, _topObject);

        //    Assert.AreEqual(6, target.CanvasItems.Count);
        //    VerifySides(target, 0, 2, 3, 4, 6);
        //    Assert.AreEqual(5, target.WindowPoints.Count);
        //}

        //[TestMethod]
        //public void Ctor_TopRightTriangle_Test()
        //{
        //    using (_mocks.Record())
        //    {
        //        SetupCommonResults(topRight: new SizeF(1000, 1000));
        //    }

        //    var target = new EditOffsetViewModel(_data, _topObject);

        //    Assert.AreEqual(4, target.CanvasItems.Count);
        //    VerifySides(target, 0, 3, 6);
        //    Assert.AreEqual(3, target.WindowPoints.Count);
        //}

        //[TestMethod]
        //public void Ctor_BottomRight_Test()
        //{
        //    using (_mocks.Record())
        //    {
        //        SetupCommonResults(bottomRight: new SizeF(500, 500));
        //    }

        //    var target = new EditOffsetViewModel(_data, _topObject);

        //    Assert.AreEqual(6, target.CanvasItems.Count);
        //    VerifySides(target, 0, 2, 4, 5, 6);
        //    Assert.AreEqual(5, target.WindowPoints.Count);
        //}

        //[TestMethod]
        //public void Ctor_BottomRightTriangle_Test()
        //{
        //    using (_mocks.Record())
        //    {
        //        SetupCommonResults(bottomRight: new SizeF(1000, 1000));
        //    }

        //    var target = new EditOffsetViewModel(_data, _topObject);

        //    Assert.AreEqual(4, target.CanvasItems.Count);
        //    VerifySides(target, 0, 2, 5);
        //    Assert.AreEqual(3, target.WindowPoints.Count);
        //}

        //[TestMethod]
        //public void Ctor_BottomLeft_Test()
        //{
        //    using (_mocks.Record())
        //    {
        //        SetupCommonResults(bottomLeft: new SizeF(500, 500));
        //    }

        //    var target = new EditOffsetViewModel(_data, _topObject);

        //    Assert.AreEqual(6, target.CanvasItems.Count);
        //    VerifySides(target, 0, 2, 4, 6, 7);
        //    Assert.AreEqual(5, target.WindowPoints.Count);
        //}

        //private void VerifySides(EditOffsetViewModel target, params int[] sides)
        //{
        //    foreach (var side in sides)
        //    {
        //        Assert.AreEqual(1, target.CanvasItems.Count(i => i.Side == side));
        //    }
        //}

        //private void SetupCommonResults(SizeF topLeft = new SizeF(), SizeF topRight = new SizeF(),
        //    SizeF bottomLeft = new SizeF(), SizeF bottomRight = new SizeF())
        //{
        //    SetupResult.For(_topObject.Dimensions).Return(_dimensions);
        //    SetupResult.For(_topObject.get_Slants(0)).Return(topLeft);
        //    SetupResult.For(_topObject.get_Slants(1)).Return(topRight);
        //    SetupResult.For(_topObject.get_Slants(2)).Return(bottomRight);
        //    SetupResult.For(_topObject.get_Slants(3)).Return(bottomLeft);
        //}
    }
}
