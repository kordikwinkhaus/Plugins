using System;
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
