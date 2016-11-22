using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowOffset.Models;

namespace WindowOffset.Tests.Models
{
    [TestClass]
    public class DimensionLayerTest
    {
        [TestMethod]
        public void Add_Test()
        {
            DimensionLayer target = new DimensionLayer();

            target.Add(new Dimension(10));
            target.Add(new Dimension(50));

            Assert.AreEqual(10, target[1].From);
        }
    }
}
