using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            target.IsExpanded = true;
            var props = target.Children;

            Assert.AreEqual(1, props.Count);
            Assert.AreEqual("Length", ((VariableViewModel)(props[0])).Name);
        }

        [TestMethod]
        public void Int32_Test()
        {
            int i = 123;
            var target = GetTarget("i", i);

            var props = target.Children;

            Assert.AreEqual(0, props.Count);
        }
    }
}
