using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowOffset.Models;

namespace WindowOffset.Tests.Models
{
    [TestClass]
    public class MainOffsetTest
    {
        [TestMethod]
        public void Offset_SubitemInherit_Test()
        {
            var target = new MainOffset();
            var subitem = new SideOffset();
            target.Add(subitem);

            target.Offset = 40;

            Assert.AreEqual(40, target.Offset);
            Assert.AreEqual(40, subitem.Offset);
            Assert.IsFalse(subitem.HasOwnValue);
        }

        [TestMethod]
        public void Offset_SubitemHasOwnValue_Test()
        {
            var target = new MainOffset();
            var subitem = new SideOffset();
            target.Add(subitem);
            subitem.Offset = 30;

            target.Offset = 40;

            Assert.AreEqual(40, target.Offset);
            Assert.AreEqual(30, subitem.Offset);
            Assert.IsTrue(subitem.HasOwnValue);
        }
    }
}
