using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowOffset.Models;

namespace WindowOffset.Tests.Models
{
    [TestClass]
    public class LineTest_Intersection
    {
        const float DELTA = 0.01f;

        [TestMethod]
        public void Intersection_Test1()
        {
            var target = new Line(1, new PointF(0, 1000), new PointF(1000, 0));
            var other = new Line(3, new PointF(0, 0), new PointF(1000, 1000));

            var intersection = target.Intersection(other);

            VerifyIntersection(new PointF(500, 500), intersection);
        }

        [TestMethod]
        public void Intersection_Test2()
        {
            var target = new Line(1, new PointF(0, 1000), new PointF(1000, 0));
            var other = new Line(2, new PointF(0, 500), new PointF(1000, 500));

            var intersection = target.Intersection(other);

            VerifyIntersection(new PointF(500, 500), intersection);
        }

        [TestMethod]
        public void Intersection_Test3()
        {
            var target = new Line(0, new PointF(0, 1000), new PointF(0, 500));
            var other = new Line(2, new PointF(500, 0), new PointF(1000, 0));

            var intersection = target.Intersection(other);

            VerifyIntersection(new PointF(0, 0), intersection);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Intersection_Paralell_Test()
        {
            var target = new Line(1, new PointF(0, 1000), new PointF(0, 1000));
            var other = new Line(2, new PointF(0, 500), new PointF(1000, 500));

            var intersection = target.Intersection(other);
        }

        private void VerifyIntersection(PointF expected, PointF actual)
        {
            Assert.AreEqual(expected.X, actual.X, DELTA);
            Assert.AreEqual(expected.Y, actual.Y, DELTA);
        }
    }
}
