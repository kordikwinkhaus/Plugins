using System.Collections.Generic;
using System.Diagnostics;
using Ctor.Models.Scripting;
using IronPython.Runtime;
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
            Assert.AreEqual("int", target.Name);
        }

        [TestMethod]
        public void Ctor_Double_Test()
        {
            var target = new TypeCacheInfo(typeof(double));

            double d = 5.67;
            Assert.AreEqual("5.67", target.GetDebugValue(d));
            Assert.IsFalse(target.HasPublicProperties);
            Assert.AreEqual("double", target.Name);
        }

        [TestMethod]
        public void Ctor_String_Test()
        {
            var target = new TypeCacheInfo(typeof(string));

            Assert.AreEqual("\"hello\"", target.GetDebugValue("hello"));
            Assert.IsFalse(target.HasPublicProperties);
            Assert.AreEqual("string", target.Name);

            var props = target.GetPublicProperties();
            Assert.AreEqual(0, props.Count); // string má veřejnou vlastnost "Length", ale tu nechceme zobrazovat
        }

        [TestMethod]
        public void Ctor_List_Test()
        {
            var target = new TypeCacheInfo(typeof(List<int>));
            var obj = new List<int> { 1, 2, 3 };

            Assert.AreEqual("System.Collections.Generic.List<int>", target.Name);
            //Assert.AreEqual("Count = 3", target.GetDebugValue(obj));
        }

        [TestMethod]
        public void Ctor_Dictionary_Test()
        {
            var target = new TypeCacheInfo(typeof(Dictionary<string, bool>));
            //var obj = new Dictionary<int, string>();

            Assert.AreEqual("System.Collections.Generic.Dictionary<string, bool>", target.Name);
            //Assert.AreEqual("Count = 3", target.GetDebugValue(obj));
        }

        [TestMethod]
        public void Ctor_PythonDictionary_PythonType_Test()
        {
            var target = new TypeCacheInfo(typeof(PythonDictionary));

            Assert.AreEqual("dict", target.Name);
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
