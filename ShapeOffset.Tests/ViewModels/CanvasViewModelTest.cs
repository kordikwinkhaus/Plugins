using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShapeOffset.ViewModels;

namespace ShapeOffset.Tests.ViewModels
{
    [TestClass]
    public class CanvasViewModelTest
    {
        CanvasViewModel GetTarget()
        {
            return new CanvasViewModel();
        }

        [TestMethod]
        public void DrawShape_CloseOnLastPoint_Test()
        {
            var target = GetTarget();

            target.NotifyMouseClick(new Point(0, 0));
            target.NotifyMouseClick(new Point(0, 100));
            target.NotifyMouseClick(new Point(100, 0));
            target.NotifyMouseClick(new Point(0, 0));

            Assert.IsTrue(target.ClosedShape);
            Assert.AreEqual(3, target.Items.Count);
        }

        [TestMethod]
        public void DrawShape_CloseByCommand_Test()
        {
            var target = GetTarget();

            target.NotifyMouseClick(new Point(0, 0));
            target.NotifyMouseClick(new Point(0, 100));
            target.NotifyMouseClick(new Point(100, 0));

            target.MousePosition = new Point(100, 100);
            target.CloseShapeCommand.Execute(null);

            Assert.IsTrue(target.ClosedShape);
            Assert.AreEqual(3, target.Items.Count);
        }
    }
}
