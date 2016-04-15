using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using EOkno.Models;
using EOkno.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserExt;

namespace EOkno.Tests.ViewModels
{
    [TestClass]
    public class PositionViewModelTest
    {
        private FakeOknaDocument _oknaDoc;

        [TestInitialize]
        public void Init()
        {
            _oknaDoc = new FakeOknaDocument();
        }

        private PositionViewModel GetTarget()
        {
            PositionViewModel target = new PositionViewModel();
            target.OknaDocument = _oknaDoc;
            ExtensionsFactory.Nacist(target);
            return target;
        }

        private PositionData GetPositionData(string xml = "<EOkno/>")
        {
            XDocument doc = XDocument.Parse(xml);
            return new PositionData(doc.Root);
        }

        [TestMethod]
        public void NovaPozice_VychoziPodleDokumentu_Test()
        {
            var target = GetTarget();
            var model = GetPositionData();
            _oknaDoc.ExtendedProperties = XElement.Parse("<UserData><EOkno s='0'><p k='olej' i='PinO' e='OliO'/><k m='1'/><k p='20'/></EOkno></UserData>");

            target.SetModel(model);
            target.SetDefaults();

            Assert.AreEqual(true, target.InheritFromDocument);
            Assert.AreEqual(false, target.NotInheritFromDocument);

            Assert.AreEqual("olej", target.VybranaPU.Kod);
            Assert.AreEqual("PinO", target.VybranaPU.VnitrniOdstin.Kod);
            Assert.AreEqual("OliO", target.VybranaPU.VnejsiOdstin.Kod);

            Assert.IsTrue(target.Komponenty[0].Vybrano);
            for (int i = 1; i < target.Komponenty.Count - 1; i++)
            {
                Assert.IsFalse(target.Komponenty[i].Vybrano);
            }
            Assert.IsTrue(target.Komponenty.Last().Vybrano);
        }

        [TestMethod]
        public void DruhaNovaPozice_VychoziPodleDokumentu_Test()
        {
            var target = GetTarget();
            var model = GetPositionData();
            _oknaDoc.ExtendedProperties = XElement.Parse("<UserData><EOkno s='0'><p k='olej' i='PinO' e='OliO'/><k m='1'/><k p='20'/></EOkno></UserData>");

            // první pozice
            target.SetModel(model);
            target.SetDefaults();

            // druhá pozice
            model = GetPositionData();
            target.SetModel(model);
            target.SetDefaults();

            Assert.AreEqual(true, target.InheritFromDocument);
            Assert.AreEqual(false, target.NotInheritFromDocument);

            Assert.AreEqual("olej", target.VybranaPU.Kod);
            Assert.AreEqual("PinO", target.VybranaPU.VnitrniOdstin.Kod);
            Assert.AreEqual("OliO", target.VybranaPU.VnejsiOdstin.Kod);

            Assert.IsTrue(target.Komponenty[0].Vybrano);
            for (int i = 1; i < target.Komponenty.Count - 1; i++)
            {
                Assert.IsFalse(target.Komponenty[i].Vybrano);
            }
            Assert.IsTrue(target.Komponenty.Last().Vybrano);
        }

        [TestMethod]
        public void NovaPozice_Vlastni_InicializovatPodleDokumentu_Test()
        {
            var target = GetTarget();
            var model = GetPositionData();
            _oknaDoc.ExtendedProperties = XElement.Parse("<UserData><EOkno s='0'><p k='olej' i='PinO' e='OliO'/><k m='1'/><k p='20'/></EOkno></UserData>");

            target.SetModel(model);
            target.SetDefaults();
            target.InheritFromDocument = false;

            Assert.AreEqual("olej", target.VybranaPU.Kod);
            Assert.AreEqual("PinO", target.VybranaPU.VnitrniOdstin.Kod);
            Assert.AreEqual("OliO", target.VybranaPU.VnejsiOdstin.Kod);

            Assert.IsTrue(target.Komponenty[0].Vybrano);
            Assert.IsTrue(target.Komponenty[0].VybranoDokument);
            for (int i = 1; i < target.Komponenty.Count - 1; i++)
            {
                Assert.IsFalse(target.Komponenty[i].Vybrano);
                Assert.IsFalse(target.Komponenty[i].VybranoDokument);
            }
            Assert.IsTrue(target.Komponenty.Last().Vybrano);
            Assert.IsTrue(target.Komponenty.Last().VybranoDokument);
        }

        [TestMethod]
        public void ExistujiciPozice_PodleDokumentu_Test()
        {
            var target = GetTarget();
            var model = GetPositionData("<EOkno doc='1'><p/></EOkno>");
            _oknaDoc.ExtendedProperties = XElement.Parse("<UserData><EOkno s='0'><p k='olej' i='PinO' e='OliO'/><k m='1'/><k p='20'/></EOkno></UserData>");

            // druhé zobrazení
            target.SetModel(model);

            Assert.IsTrue(target.InheritFromDocument);
            Assert.AreEqual("olej", target.VybranaPU.Kod);
            Assert.AreEqual("PinO", target.VybranaPU.VnitrniOdstin.Kod);
            Assert.AreEqual("OliO", target.VybranaPU.VnejsiOdstin.Kod);

            Assert.IsTrue(target.Komponenty[0].Vybrano);
            for (int i = 1; i < target.Komponenty.Count - 1; i++)
            {
                Assert.IsFalse(target.Komponenty[i].Vybrano);
            }
            Assert.IsTrue(target.Komponenty.Last().Vybrano);
        }

        [TestMethod]
        public void ExistujiciPozice_Vlastni_SignalizaceRozdilu_Test()
        {
            var target = GetTarget();
            var model = GetPositionData("<EOkno doc='0'><p k='olej' i='PinO' e='OliO'/><k m='1'/></EOkno>");
            _oknaDoc.ExtendedProperties = XElement.Parse("<UserData><EOkno s='0'><p k='olej' i='PinO' e='OliO'/><k m='1'/><k p='20'/></EOkno></UserData>");

            // druhé zobrazení
            target.SetModel(model);

            Assert.IsFalse(target.InheritFromDocument);
            Assert.AreEqual("olej", target.VybranaPU.Kod);
            Assert.AreEqual("PinO", target.VybranaPU.VnitrniOdstin.Kod);
            Assert.AreEqual("OliO", target.VybranaPU.VnejsiOdstin.Kod);

            Assert.IsTrue(target.Komponenty[0].Vybrano);
            for (int i = 1; i < target.Komponenty.Count; i++)
            {
                Assert.IsFalse(target.Komponenty[i].Vybrano);
            }

            Assert.IsTrue(target.Komponenty[0].VybranoDokument);
            for (int i = 1; i < target.Komponenty.Count - 1; i++)
            {
                Assert.IsFalse(target.Komponenty[i].VybranoDokument);
            }
            Assert.IsTrue(target.Komponenty.Last().VybranoDokument);
        }
    }
}
