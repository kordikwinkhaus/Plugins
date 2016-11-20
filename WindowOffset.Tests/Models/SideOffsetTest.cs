using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowOffset.Models;

namespace WindowOffset.Tests.Models
{
    [TestClass]
    public class SideOffsetTest
    {
        [TestMethod]
        public void Offset_Test()
        {
            var target = new SideOffset();

            target.Offset = 50;

            Assert.AreEqual(50, target.Offset);
            Assert.IsTrue(target.HasOwnValue);
        }

        [TestMethod]
        public void ResetValueCommand_Test()
        {
            var target = new SideOffset();
            target.TrySetParentOffset(50);
            target.Offset = 30;

            target.ResetOffset();

            Assert.AreEqual(50, target.Offset);
            Assert.IsFalse(target.HasOwnValue);
        }
    }
}
