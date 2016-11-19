using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowOffset.ViewModels;

namespace WindowOffset.Tests.ViewModels
{
    [TestClass]
    public class OffsetItemViewModelTest
    {
        [TestMethod]
        public void Offset_Test()
        {
            var target = new OffsetItemViewModel();

            target.Offset = 50;

            Assert.AreEqual(50, target.Offset);
            Assert.IsTrue(target.HasOwnValue);
        }

        [TestMethod]
        public void ResetValueCommand_Test()
        {
            var target = new OffsetItemViewModel();
            target.TrySetParentValue(50);
            target.Offset = 30;

            target.ResetValueCommand.Execute(null);

            Assert.AreEqual(50, target.Offset);
            Assert.IsFalse(target.HasOwnValue);
        }
    }
}
