using System.Xml.Linq;
using EOkno.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EOkno.Tests.Models
{
    [TestClass]
    public class ColorsAndComponentsTest
    {
        private XDocument _currentDoc;

        private ColorsAndComponents GetTarget(string xml = "<EOkno/>")
        {
            _currentDoc = XDocument.Parse(xml);
            var target = new ColorsAndComponents(_currentDoc.Root);
            return target;
        }

        [TestMethod]
        public void Ctor_Test()
        {
            string xml = @"<EOkno>
  <p k='olej' e='PinO' en='Pinie' i='OliO' in='Oliva'/>
    <k m='1'/>
    <k p='2'/>
</EOkno>";
            var target = GetTarget(xml);

            Assert.AreEqual("olej", target.PovrchovaUpravaKod);
            Assert.AreEqual("PinO", target.OdstinExterierKod);
            Assert.AreEqual("OliO", target.OdstinInterierKod);
            Assert.IsTrue(target.JeVybranMaterial("1"));
            Assert.IsTrue(target.JeVybranaPrace("2"));

            Assert.IsFalse(target.JeVybranMaterial("91"));
            Assert.IsFalse(target.JeVybranaPrace("92"));
        }

        [TestMethod]
        public void SetOdstinExterier_Test()
        {
            var target = GetTarget();

            target.PovrchovaUpravaKod = "olej";
            target.SetOdstinExterier("O_mah", "Mahagon");
            target.SetOdstinInterier("O_pal", "Palisandr");

            string xml = _currentDoc.ToString();
            Assert.IsTrue(xml.Contains("Mahagon"));
            Assert.IsTrue(xml.Contains("Palisandr"));

            var target2 = GetTarget(xml);
            Assert.AreEqual(target.PovrchovaUpravaKod, target2.PovrchovaUpravaKod);
            Assert.AreEqual(target.OdstinExterierKod, target2.OdstinExterierKod);
            Assert.AreEqual(target.OdstinInterierKod, target2.OdstinInterierKod);
        }

        [TestMethod]
        public void ZmenitVyberKomponenty_InternalState_Test()
        {
            var target = GetTarget();
            string komponenta = "33";

            Assert.IsFalse(target.JeVybranMaterial(komponenta));

            target.ZmenitVyberMaterialu(komponenta, true);

            Assert.IsTrue(target.JeVybranMaterial(komponenta));

            target.ZmenitVyberMaterialu(komponenta, false);

            Assert.IsFalse(target.JeVybranMaterial(komponenta));
        }

        [TestMethod]
        public void ZmenitVyberKomponenty_SaveToXml_Test()
        {
            var target = GetTarget();
            string komponenta = "33";

            target.ZmenitVyberMaterialu(komponenta, true);

            string xml = _currentDoc.ToString();
            var target2 = GetTarget(xml);

            Assert.IsTrue(target2.JeVybranMaterial(komponenta));
            target2.ZmenitVyberMaterialu(komponenta, false);

            string xml2 = _currentDoc.ToString();
            var target3 = GetTarget(xml2);

            Assert.IsFalse(target3.JeVybranMaterial(komponenta));
        }

        [TestMethod]
        public void ZmenitVyberPrace_InternalState_Test()
        {
            var target = GetTarget();
            string prace = "7";

            Assert.IsFalse(target.JeVybranaPrace(prace));

            target.ZmenitVyberPrace(prace, true);

            Assert.IsTrue(target.JeVybranaPrace(prace));

            target.ZmenitVyberPrace(prace, false);

            Assert.IsFalse(target.JeVybranaPrace(prace));
        }
    }
}
