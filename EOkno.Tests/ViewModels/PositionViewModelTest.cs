using System.Collections.Generic;
using System.Xml.Linq;
using EOkno.Models;
using EOkno.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            target.PovrchoveUpravy.Add(new PovrchovaUpravaViewModel("bez", "bez PU"));
            PovrchovaUpravaViewModel olej = new PovrchovaUpravaViewModel("olej", "olej");
            olej.Odstiny = new List<OdstinViewModel>();
            olej.Odstiny.Add(new OdstinViewModel("o1", "Odst 1"));
            olej.Odstiny.Add(new OdstinViewModel("o2", "Odst 2"));
            target.PovrchoveUpravy.Add(olej);

            target.Komponenty.Add(new KomponentaViewModel("Mat 1", "M1", null, target));
            target.Komponenty.Add(new KomponentaViewModel("Mat 2", "M2", null, target));
            target.Komponenty.Add(new KomponentaViewModel("Práce 1", null, "P1", target));

            return target;
        }

        private PositionData GetDefaultPositionData()
        {
            XDocument doc = XDocument.Parse("<EOkno/>");
            return new PositionData(doc.Root);
        }

        [TestMethod]
        public void NovaPozice_VychoziPodleDokumentu_Test()
        {
            var target = GetTarget();
            var model = GetDefaultPositionData();
            _oknaDoc.ExtendedProperties = XElement.Parse("<UserData><EOkno s='0'><p k='olej' i='o1' e='o2'/><k m='M1'/><k p='P1'/></EOkno></UserData>");

            target.SetModel(model);
            target.SetDefaults();

            Assert.AreEqual(true, target.InheritFromDocument);
            Assert.AreEqual(false, target.NotInheritFromDocument);

            Assert.AreEqual("olej", target.VybranaPU.Kod);
            Assert.AreEqual("o1", target.VybranaPU.VnitrniOdstin.Kod);
            Assert.AreEqual("o2", target.VybranaPU.VnejsiOdstin.Kod);

            bool[] ocekavaneStavy = new [] { true, false, true };
            Assert.AreEqual(ocekavaneStavy.Length, target.Komponenty.Count);
            for (int i = 0; i < ocekavaneStavy.Length; i++)
            {
                Assert.AreEqual(ocekavaneStavy[i], target.Komponenty[i].Vybrano);
            }
        }

        [TestMethod]
        public void DruhaNovaPozice_VychoziPodleDokumentu_Test()
        {
            var target = GetTarget();
            var model = GetDefaultPositionData();
            _oknaDoc.ExtendedProperties = XElement.Parse("<UserData><EOkno s='0'><p k='olej' i='o1' e='o2'/><k m='M1'/><k p='P1'/></EOkno></UserData>");

            // první pozice
            target.SetModel(model);
            target.SetDefaults();

            // druhá pozice
            model = GetDefaultPositionData();
            target.SetModel(model);
            target.SetDefaults();

            Assert.AreEqual(true, target.InheritFromDocument);
            Assert.AreEqual(false, target.NotInheritFromDocument);

            Assert.AreEqual("olej", target.VybranaPU.Kod);
            Assert.AreEqual("o1", target.VybranaPU.VnitrniOdstin.Kod);
            Assert.AreEqual("o2", target.VybranaPU.VnejsiOdstin.Kod);

            bool[] ocekavaneStavy = new [] { true, false, true };
            Assert.AreEqual(ocekavaneStavy.Length, target.Komponenty.Count);
            for (int i = 0; i < ocekavaneStavy.Length; i++)
            {
                Assert.AreEqual(ocekavaneStavy[i], target.Komponenty[i].Vybrano);
            }
        }
    }
}
