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

        [TestMethod]
        public void IsEmpty_Test()
        {
            var elem = XElement.Parse("<EOkno/>");
            var target = new PositionData(elem);
            string kod = "4";

            Assert.IsFalse(target.IsEmpty()); // je to podle dokumentu

            target.PodleDokumentu = false;

            Assert.IsTrue(target.IsEmpty());

            target.ZmenitVyberMaterialu(kod, true);

            Assert.IsFalse(target.IsEmpty());

            target.ZmenitVyberMaterialu(kod, false);

            Assert.IsTrue(target.IsEmpty());

            target.ZmenitVyberPrace(kod, true);

            Assert.IsFalse(target.IsEmpty());

            target.ZmenitVyberPrace(kod, false);

            Assert.IsTrue(target.IsEmpty());

            target.PovrchovaUpravaKod = kod;

            Assert.IsFalse(target.IsEmpty());

            target.PovrchovaUpravaKod = null;

            Assert.IsTrue(target.IsEmpty());
        }
    }
}
