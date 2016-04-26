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
            Assert.AreEqual("Count = 3", target.GetDebugValue(obj));
        }

        [TestMethod]
        public void Ctor_Dictionary_Test()
        {
            var target = new TypeCacheInfo(typeof(Dictionary<string, bool>));
            var obj = new Dictionary<string, bool>
            {
                { "test", true }
            };

            Assert.AreEqual("System.Collections.Generic.Dictionary<string, bool>", target.Name);
            Assert.AreEqual("Count = 1", target.GetDebugValue(obj));

            // u generic nevracet "ToString()", ale název typu, pokud není ToString() přetížena
            var comparer = obj.Comparer;
            var target2 = new TypeCacheInfo(comparer.GetType());
            string name = target2.Name;
            Assert.AreEqual(name, target2.GetDebugValue(comparer));
        }

        [TestMethod]
        public void Ctor_Array_Test()
        {
            var target = new TypeCacheInfo(typeof(bool[]));
            var obj = new bool[] { false };

            Assert.AreEqual("bool[]", target.Name);
        }

        [TestMethod]
        public void Ctor_MultiDimensionalArray_Test()
        {
            var target = new TypeCacheInfo(typeof(int[,,]));
            int[,,] array1 = new int[4, 2, 3];

            Assert.AreEqual("int[,,]", target.Name);
        }

        [TestMethod]
        public void Ctor_JaggedArray_Test()
        {
            var target = new TypeCacheInfo(typeof(float[][]));
            float[][] array = new float[4][];

            Assert.AreEqual("float[][]", target.Name);
        }

        [TestMethod]
        public void DebuggerDisplay_NonQuote_Test()
        {
            var target = new TypeCacheInfo(typeof(DebuggerDisplayTest2));
            var obj = new DebuggerDisplayTest2 { Prop = "hello" };

            Assert.AreEqual("TEST: hello", target.GetDebugValue(obj));
        }

        [TestMethod]
        public void DebuggerDisplay_MultipleProps_Test()
        {
            var target = new TypeCacheInfo(typeof(DebuggerDisplayTest4));
            var obj = new DebuggerDisplayTest4();

            Assert.AreEqual("\"hello\" world!", target.GetDebugValue(obj));
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

        [TestMethod]
        public void DebuggerDisplay_TypeName_Test()
        {
            var target = new TypeCacheInfo(typeof(DebugerBrowsableTest));

            Assert.AreEqual("MyClass", target.Name);
        }
    }

    [DebuggerDisplay("{Visible}", Type = "MyClass")]
    class DebugerBrowsableTest
    {
        public int Visible { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Hidden { get; set; }
    }

    [DebuggerDisplay("TEST: {Prop,nq}")]
    class DebuggerDisplayTest2
    {
        public string Prop { get; set; }
    }

    [DebuggerDisplay("{Prop1} {Prop2,nq}!")]
    class DebuggerDisplayTest4
    {
        public string Prop1 { get; set; } = "hello";
        public string Prop2 { get; set; } = "world";
    }
}
