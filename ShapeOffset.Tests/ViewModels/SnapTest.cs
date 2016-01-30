using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShapeOffset.ViewModels;

namespace ShapeOffset.Tests.ViewModels
{
    [TestClass]
    public class SnapTest
    {
        [TestMethod]
        public void SnapToGrid_IntUp_Test()
        {
            int orig = (int)(Snap.GRID_SIZE * 5.75);
            int expected = Snap.GRID_SIZE * 6;

            Assert.AreEqual(expected, Snap.ToGrid(orig));
        }

        [TestMethod]
        public void SnapToGrid_IntDown_Test()
        {
            int orig = (int)(Snap.GRID_SIZE * 5.35);
            int expected = Snap.GRID_SIZE * 5;

            Assert.AreEqual(expected, Snap.ToGrid(orig));
        }

        [TestMethod]
        public void SnapToGrid_DoubleUp_Test()
        {
            double orig = Snap.GRID_SIZE * 5.95;
            double expected = Snap.GRID_SIZE * 6;

            Assert.AreEqual(expected, Snap.ToGrid(orig), 0.0001);
        }

        [TestMethod]
        public void SnapToGrid_DoubleDown_Test()
        {
            double orig = Snap.GRID_SIZE * 5.05;
            double expected = Snap.GRID_SIZE * 5;

            Assert.AreEqual(expected, Snap.ToGrid(orig), 0.0001);
        }
    }
}
