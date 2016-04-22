using System.Collections.Generic;
using System.Linq;
using Ctor.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ctor.Tests.ViewModels
{
    [TestClass]
    public class VariablesViewModelTest
    {
        [TestMethod]
        public void Ctor_Simple_Test()
        {
            Dictionary<object, object> vars = new Dictionary<object, object>
            {
                { "i", 5 },
                { "s", "hello world" },
                { "b", true }
            };
            var target = new VariablesViewModel(vars);

            Assert.AreEqual(3, target.Variables.Count);
            Assert.AreEqual(1, target.Variables.Count(v => v.Name == "i" && v.Value == "5"));
            Assert.AreEqual(1, target.Variables.Count(v => v.Name == "s" && v.Value == "\"hello world\""));
            Assert.AreEqual(1, target.Variables.Count(v => v.Name == "b" && v.Value == "True"));
        }

        [TestMethod]
        public void Update_Simple_Test()
        {
            Dictionary<object, object> vars = new Dictionary<object, object>
            {
                { "i", 5 },
                { "s", "hello world" },
                { "b", true }
            };
            var target = new VariablesViewModel(vars);

            vars.Remove("i");
            vars.Add("d", 5.5m);
            vars["s"] = "hello";

            target.Update(vars);

            Assert.AreEqual(3, target.Variables.Count);
            Assert.AreEqual(1, target.Variables.Count(v => v.Name == "s" && v.Value == "\"hello\""));
            Assert.AreEqual(1, target.Variables.Count(v => v.Name == "b" && v.Value == "True"));
            Assert.AreEqual(1, target.Variables.Count(v => v.Name == "d" && v.Value == "5.5"));
        }
    }
}
