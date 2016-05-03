using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ctor.Models.Scripting;
using Ctor.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ctor.Tests.ViewModels
{
    [TestClass]
    public class VariableViewModelTest
    {
        private VariableViewModel GetTarget(string name, object value)
        {
            return new VariableViewModel(name, value);
        }

        [TestMethod]
        public void String_Test()
        {
            string s = "hello";
            var target = GetTarget("s", s);

            var props = target.Children;

            Assert.AreEqual(0, props.Count);
        }

        [TestMethod]
        public void Int32_Test()
        {
            int i = 123;
            var target = GetTarget("i", i);

            var props = target.Children;

            Assert.AreEqual(0, props.Count);
        }

        [TestMethod]
        public void Null_Test()
        {
            var target = GetTarget("n", null);

            Assert.AreEqual(0, target.Children.Count);
            var n = new List<int> { 4, 5, 6 };

            target.Update(n);

            Assert.AreEqual(1, target.Children.Count);
            Assert.IsTrue(target.HasDummyChild);

            target.IsExpanded = true;

            Assert.AreEqual(3, target.Children.Count);
        }

        [TestMethod]
        public void GetPropertyValueThrowsException_Test()
        {
            var obj = new InvalidatedClass();
            var target = GetTarget("obj", obj);

            target.IsExpanded = true;
            var props = target.Children;

            Assert.AreEqual(1, props.Count);

            VariableViewModel vm = (VariableViewModel)(props[0]);
            Assert.AreEqual("int", vm.VariableType);
            Assert.AreEqual("Call threw an exception of type 'System.InvalidOperationException'.", vm.Value);
            Assert.AreEqual(0, vm.Children.Count);
        }

        [TestMethod]
        public void MyClass_Update_Test()
        {
            var obj = new MyClass
            {
                Nazev = "moje",
                Pocet = 12
            };
            var target = GetTarget("obj", obj);

            target.IsExpanded = true;

            Assert.AreEqual("moje: 12", target.Value);
            Verify(target.Children[0], "Nazev", "\"moje\"");
            Verify(target.Children[1], "Pocet", "12");

            obj.Nazev = "hello";
            obj.Pocet = 33;

            target.Update(obj);

            Assert.AreEqual("hello: 33", target.Value);
            Verify(target.Children[0], "Nazev", "\"hello\"");
            Verify(target.Children[1], "Pocet", "33");
        }

        [TestMethod]
        public void List_Test()
        {
            ArrayList obj = new ArrayList
            {
                "val0",
                "val1",
                "val2"
            };
            var target = GetTarget("obj", obj);

            target.IsExpanded = true;

            Assert.AreEqual(3, target.Children.Count);
            for (int i = 0; i < target.Children.Count; i++)
            {
                Verify(target.Children[i], "[" + i + "]", "\"val" + i + "\"");
            }

            obj[0] = "hello";
            obj.Add("val3");

            target.Update(obj);

            Assert.AreEqual(4, target.Children.Count);
            Verify(target.Children[0], "[0]", "\"hello\"");
            for (int i = 1; i < target.Children.Count; i++)
            {
                Verify(target.Children[i], "[" + i + "]", "\"val" + i + "\"");
            }

            obj.RemoveAt(0);
            target.Update(obj);

            Assert.AreEqual(3, target.Children.Count);
            for (int i = 0; i < target.Children.Count; i++)
            {
                Verify(target.Children[i], "[" + i + "]", "\"val" + (i + 1) + "\"");
            }

            target.Update(null);

            Assert.AreEqual(0, target.Children.Count);
            Assert.AreEqual(TypeCacheInfo.NULL, target.Value);
        }

        [TestMethod]
        public void ListChangedToDict_Test()
        {
            List<int> list = new List<int> { 10, 11 };
            Dictionary<string, double> dict = new Dictionary<string, double>
            {
                { "a1", 5.5 }
            };

            var target = GetTarget("o", list);
            target.IsExpanded = true;

            target.Update(dict);
            Assert.AreEqual(1, target.Children.Count);
            Assert.AreEqual("[\"a1\"]", ((VariableViewModel)(target.Children[0])).Name);
        }

        [TestMethod]
        public void ObjectChangedToList_Test()
        {
            var obj = new MyClass { Pocet = 1, Nazev = "Produkt" };
            List<int> list = new List<int> { 10, 11 };

            var target = GetTarget("o", obj);
            target.IsExpanded = true;

            target.Update(list);
            Assert.AreEqual(2, target.Children.Count);
            Assert.AreEqual("10", ((VariableViewModel)(target.Children[0])).Value);
            Assert.AreEqual("11", ((VariableViewModel)(target.Children[1])).Value);
        }

        [TestMethod]
        public void StringChangedToList_Test()
        {
            var target = GetTarget("a", "hello");

            Assert.AreEqual(0, target.Children.Count);

            List<int> list = new List<int>();
            target.Update(list);

            Assert.AreEqual(1, target.Children.Count);
            Assert.IsTrue(target.HasDummyChild);
        }

        [TestMethod]
        public void GenericList_Test()
        {
            List<int> obj = new List<int> { 10, 11, 12, 13 };
            var target = GetTarget("obj", obj);

            target.IsExpanded = true;

            Assert.AreEqual(4, target.Children.Count);
            for (int i = 0; i < target.Children.Count; i++)
            {
                Verify(target.Children[i], "[" + i + "]", (10 + i).ToString());
            }
        }

        [TestMethod]
        public void Hashtable_Test()
        {
            Hashtable dict = new Hashtable
            {
                { "a", 55 },
                { "c", 77 },
                { "b", 66 }
            };
            var target = GetTarget("dict", dict);

            target.IsExpanded = true;

            Assert.AreEqual(3, target.Children.Count);
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("a")), "[\"a\"]", "55");
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("b")), "[\"b\"]", "66");
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("c")), "[\"c\"]", "77");
        }

        [TestMethod]
        public void Dictionary_Test()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>
            {
                { "a", 55 },
                { "b", 66 }
            };
            var target = GetTarget("dict", dict);

            target.IsExpanded = true;

            Assert.AreEqual(2, target.Children.Count);
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("a")), "[\"a\"]", "55");
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("b")), "[\"b\"]", "66");

            dict["a"] = 11;
            dict["c"] = 77;
            target.Update(dict);

            Assert.AreEqual(3, target.Children.Count);
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("a")), "[\"a\"]", "11");
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("b")), "[\"b\"]", "66");
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("c")), "[\"c\"]", "77");

            dict.Remove("a");
            target.Update(dict);

            Assert.AreEqual(2, target.Children.Count);
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("b")), "[\"b\"]", "66");
            Verify(target.Children.Cast<VariableViewModel>().Single(c => c.Name.Contains("c")), "[\"c\"]", "77");

            target.Update(null);

            Assert.AreEqual(0, target.Children.Count);
            Assert.AreEqual(TypeCacheInfo.NULL, target.Value);
        }

        private void Verify(TreeViewItemViewModel treeViewItemViewModel, string name, string value)
        {
            VariableViewModel vm = (VariableViewModel)treeViewItemViewModel;
            Assert.AreEqual(name, vm.Name);
            Assert.AreEqual(value, vm.Value);
        }
    }

    class MyClass
    {
        public int Pocet { get; set; }

        public string Nazev { get; set; }

        public override string ToString()
        {
            return Nazev + ": " + Pocet;
        }
    }

    class InvalidatedClass
    {
        public int Prop
        {
            get { throw new InvalidOperationException("message"); }
        }
    }
}
