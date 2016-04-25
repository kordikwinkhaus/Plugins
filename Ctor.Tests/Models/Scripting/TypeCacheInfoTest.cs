using System.Diagnostics;
using Ctor.Models.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ctor.Tests.Models.Scripting
{
    [TestClass]
    public class TypeCacheInfoTest
    {
        [TestMethod]
        public void Ctor_Int32_Test()
        {
            var target = new TypeCacheInfo(typeof(int));

            Assert.AreEqual("5", target.GetDebugValue(5));
            Assert.IsFalse(target.HasPublicProperties);
        }

        [TestMethod]
        public void Ctor_Double_Test()
        {
            var target = new TypeCacheInfo(typeof(double));

            double d = 5.67;
            Assert.AreEqual("5.67", target.GetDebugValue(d));
            Assert.IsFalse(target.HasPublicProperties);
        }

        [TestMethod]
        public void Ctor_String_Test()
        {
            var target = new TypeCacheInfo(typeof(string));

            Assert.AreEqual("\"hello\"", target.GetDebugValue("hello"));
            Assert.IsTrue(target.HasPublicProperties);

            var props = target.GetPublicProperties();
            Assert.AreEqual(1, props.Count);
            Assert.AreEqual("Length", props[0].Name);
        }

        [TestMethod]
        public void DebuggerBrowsable_Test()
        {
            var target = new TypeCacheInfo(typeof(DebugerBrowsableTest));

            var props = target.GetPublicProperties();
            Assert.AreEqual(1, props.Count);
            Assert.AreEqual("Visible", props[0].Name);
        }
    }

    class DebugerBrowsableTest
    {
        public int Visible { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Hidden { get; set; }
    }
}
