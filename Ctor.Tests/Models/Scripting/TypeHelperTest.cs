using System.Collections.Generic;
using Ctor.Models.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ctor.Tests.Models.Scripting
{
    [TestClass]
    public class TypeHelperTest
    {
        [TestMethod]
        public void OpenGenerics_Test()
        {
            Assert.IsTrue(TypeHelper.ImplementsInterface(typeof(List<int>), typeof(IList<>)));
            Assert.IsTrue(TypeHelper.ImplementsInterface(typeof(Dictionary<int, string>), typeof(IDictionary<,>)));
            Assert.IsFalse(TypeHelper.ImplementsInterface(typeof(List<string>), typeof(IDictionary<,>)));
        }
    }
}
