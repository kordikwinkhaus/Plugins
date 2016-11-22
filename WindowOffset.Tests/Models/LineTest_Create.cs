using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowOffset.Models;

namespace WindowOffset.Tests.Models
{
    [TestClass]
    public class LineTest_Create
    {
        const float DELTA = 0.01f;

        [TestMethod]
        public void Create_InvalidLines_Test()
        {
            List<SideOffset> invalids = new List<SideOffset>
            {
                // left
                new SideOffset // not vertical
                {
                    Start = new PointF(0, 1000),
                    End = new PointF(10, 0)
                },
                new SideOffset // bad direction
                {
                    Start = new PointF(0, 0),
                    End = new PointF(0, 1000)
                },

                // top left
                new SideOffset
                {
                    Start = new PointF(0, 1000),
                    End = new PointF(-1000, 0),
                    Side = 1
                },
                new SideOffset
                {
                    Start = new PointF(0, 1000),
                    End = new PointF(1000, 2000),
                    Side = 1
                },

                // top
                new SideOffset // not horizontal
                {
                    Start = new PointF(0, 1),
                    End = new PointF(1200, 0),
                    Side = 2
                },
                new SideOffset // bad direction
                {
                    Start = new PointF(1400, 0),
                    End = new PointF(1200, 0),
                    Side = 2
                },

                // top right
                new SideOffset
                {
                    Start = new PointF(0, 1000),
                    End = new PointF(-1000, 2000),
                    Side = 3
                },
                new SideOffset
                {
                    Start = new PointF(0, 1000),
                    End = new PointF(1000, -2000),
                    Side = 3
                },

                // right
                new SideOffset // not vertical
                {
                    Start = new PointF(801, 0),
                    End = new PointF(800, 1000),
                    Side = 4
                },
                new SideOffset // bad direction
                {
                    Start = new PointF(800, 2000),
                    End = new PointF(800, 1000),
                    Side = 4
                },

                // bottom right
                new SideOffset
                {
                    Start = new PointF(100, 500),
                    End = new PointF(500, 1000),
                    Offset = 100,
                },
                new SideOffset
                {
                    Start = new PointF(1000, 500),
                    End = new PointF(500, 499),
                    Offset = 100,
                },

                // bottom
                new SideOffset // not horizontal
                {
                    Start = new PointF(1200, 600),
                    End = new PointF(0, 601),
                    Side = 6
                },
                new SideOffset // bad direction
                {
                    Start = new PointF(1200, 600),
                    End = new PointF(2000, 600),
                    Side = 6
                },

                // bottom left
                new SideOffset
                {
                    Start = new PointF(-500, 1000),
                    End = new PointF(0, 500),
                    Side = 7
                },
                new SideOffset
                {
                    Start = new PointF(500, 1000),
                    End = new PointF(0, 5000),
                    Side = 7
                }
            };

            foreach (var invalid in invalids)
            {
                try
                {
                    var result = Line.Create(invalid);
                    Assert.Fail();
                }
                catch (ArgumentException) { /* ok */ }
            }
        }

        [TestMethod]
        public void Create_Left_Test()
        {
            SideOffset offset = new SideOffset
            {
                Start = new PointF(0, 1000),
                End = new PointF(0, 0),
                Offset = 50
            };

            var target = Line.Create(offset);

            VerifyLine(target, new PointF(50, 1000), new PointF(50, 0));
        }

        [TestMethod]
        public void Create_TopLeft_Test()
        {
            SideOffset offset = new SideOffset
            {
                Start = new PointF(0, 1000),
                End = new PointF(1000, 0),
                Offset = 50,
                Side = 1
            };

            var target = Line.Create(offset);

            VerifyLine(target, new PointF(35.355f, 1035.355f), new PointF(1035.355f, 35.355f));
        }

        [TestMethod]
        public void Create_Top_Test()
        {
            SideOffset offset = new SideOffset
            {
                Start = new PointF(0, 0),
                End = new PointF(1200, 0),
                Offset = 5,
                Side = 2
            };

            var target = Line.Create(offset);

            VerifyLine(target, new PointF(0, 5), new PointF(1200, 5));
        }

        [TestMethod]
        public void Create_TopRight_Test()
        {
            SideOffset offset = new SideOffset
            {
                Start = new PointF(0, 1000),
                End = new PointF(1000, 2000),
                Offset = 50,
                Side = 3
            };

            var target = Line.Create(offset);

            VerifyLine(target, new PointF(-35.355f, 1035.355f), new PointF(964.645f, 2035.355f));
        }

        [TestMethod]
        public void Create_Right_Test()
        {
            SideOffset offset = new SideOffset
            {
                Start = new PointF(800, 0),
                End = new PointF(800, 1000),
                Offset = 100,
                Side = 4
            };

            var target = Line.Create(offset);

            VerifyLine(target, new PointF(700, 0), new PointF(700, 1000));
        }

        [TestMethod]
        public void Create_BottomRight_Test()
        {
            SideOffset offset = new SideOffset
            {
                Start = new PointF(1000, 500),
                End = new PointF(500, 1000),
                Offset = 100,
                Side = 5
            };

            var target = Line.Create(offset);

            VerifyLine(target, new PointF(929.29f, 429.29f), new PointF(429.29f, 929.29f));
        }

        [TestMethod]
        public void Create_Bottom_Test()
        {
            SideOffset offset = new SideOffset
            {
                Start = new PointF(1200, 600),
                End = new PointF(0, 600),
                Offset = 35,
                Side = 6
            };

            var target = Line.Create(offset);

            VerifyLine(target, new PointF(1200, 565), new PointF(0, 565));
        }

        [TestMethod]
        public void Create_BottomLeft_Test()
        {
            SideOffset offset = new SideOffset
            {
                Start = new PointF(500, 1000),
                End = new PointF(0, 500),
                Offset = 100,
                Side = 7
            };

            var target = Line.Create(offset);

            VerifyLine(target, new PointF(570.71f, 929.29f), new PointF(70.71f, 429.29f));
        }

        private void VerifyLine(Line target, PointF start, PointF end)
        {
            Assert.AreEqual(start.X, target.Start.X, DELTA);
            Assert.AreEqual(start.Y, target.Start.Y, DELTA);
            Assert.AreEqual(end.X, target.End.X, DELTA);
            Assert.AreEqual(end.Y, target.End.Y, DELTA);
        }
    }
}
