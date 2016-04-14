using System.Xml.Linq;
using EOkno.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EOkno.Tests.Models
{
    [TestClass]
    public class PositionDataTest
    {
        [TestMethod]
        public void Ctor_EmptyElem_Test()
        {
            var elem = XElement.Parse("<EOkno/>");
            var target = new PositionData(elem);

            Assert.IsTrue(target.PodleDokumentu);
        }

        [TestMethod]
        public void Ctor_False_Test()
        {
            var elem = XElement.Parse("<EOkno doc='0'/>");
            var target = new PositionData(elem);

            Assert.IsFalse(target.PodleDokumentu);
        }

        [TestMethod]
        public void Ctor_True_Test()
        {
            var elem = XElement.Parse("<EOkno doc='1'/>");
            var target = new PositionData(elem);

            Assert.IsTrue(target.PodleDokumentu);
        }

        [TestMethod]
        public void PodleDokumentu_Test()
        {
            var elem = XElement.Parse("<EOkno/>");
            var target = new PositionData(elem);

            target.PodleDokumentu = true;

            var target2 = new PositionData(elem);

            Assert.IsTrue(target.PodleDokumentu);
        }
    }
}
