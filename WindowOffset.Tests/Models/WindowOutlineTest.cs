using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WHOkna;
using WindowOffset.Models;

namespace WindowOffset.Tests.Models
{
    [TestClass]
    public class WindowOutlineTest
    {
        const float DELTA = 0.01f;

        [TestMethod]
        public void Scale_Test()
        {
            var target = new WindowOutline
            {
                Size = new SizeF(1000, 2000),
                TopLeft = new SizeF(10, 20),
                TopRight = new SizeF(30, 40),
                BottomRight = new SizeF(50, 60),
                BottomLeft = new SizeF(70, 80)
            };

            var result = target.Scale(2);

            VerifySize(new SizeF(2000, 4000), result.Size);
            VerifySize(new SizeF(20, 40), result.TopLeft);
            VerifySize(new SizeF(60, 80), result.TopRight);
            VerifySize(new SizeF(100, 120), result.BottomRight);
            VerifySize(new SizeF(140, 160), result.BottomLeft);
        }

        [TestMethod]
        public void ApplyTo_ScaleToNearestEvenDim_Test()
        {
            var target = new WindowOutline
            {
                Size = new SizeF(799.181641f, 799.1816f),
                TopLeft = new SizeF(399.59082f, 399.59082f),
                TopRight = new SizeF(399.59082f, 399.590851f),
                BottomRight = new SizeF(399.590851f, 399.590729f),
                BottomLeft = new SizeF(399.59082f, 399.590759f)
            };

            var topObject = new MyTopObject();
            target.ApplyTo(topObject);

            VerifySize(new SizeF(800, 800), topObject.Dimensions.Size);
            var slant = new SizeF(400, 400);
            for (int i = 0; i < 4; i++)
            {
                VerifySize(slant, topObject.get_Slants(i));
            }
        }

        [TestMethod]
        public void ApplyTo_ScaleToNearestEvenDim_Test2()
        {
            var target = new WindowOutline
            {
                Size = new SizeF(902.917969f, 902.917969f),
                TopLeft = new SizeF(451.458954f, 902.917969f),
                TopRight = new SizeF(451.459f, 902.917969f)
            };

            var topObject = new MyTopObject();
            target.ApplyTo(topObject);

            VerifySize(new SizeF(902, 902), topObject.Dimensions.Size);
            var slant = new SizeF(451, 902);
            VerifySize(slant, topObject.get_Slants(0));
            VerifySize(slant, topObject.get_Slants(1));
            VerifySize(SizeF.Empty, topObject.get_Slants(2));
            VerifySize(SizeF.Empty, topObject.get_Slants(3));
        }

        private void VerifySize(SizeF expected, SizeF actual)
        {
            Assert.AreEqual(expected.Width, actual.Width, DELTA);
            Assert.AreEqual(expected.Height, actual.Height, DELTA);
        }

        class MyTopObject : ITopObject
        {
            public RectangleF Dimensions { get; set; }

            public bool Update(bool update_children)
            {
                return true;
            }

            private SizeF[] _slants = new SizeF[4];
            public SizeF get_Slants(int value)
            {
                return _slants[value];
            }

            public void set_Slants(int value1, SizeF value2)
            {
                _slants[value1] = value2;
            }

            public void Invalidate()
            {
            }

            public void CheckPoint()
            {
            }

            #region NotImplemented

            public IEnumerable<IPart> AdditionalParts
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerable<IArea> Areas
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string Article
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string AssemblyHint
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerable<IBar> Bars
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IElData[] CalculationElements
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IElData[] CalculationElementsExpanded
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public XElement CalculationTrace
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool CanDelete
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool CanPaste
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public int Color
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public XElement CurrentCalculationTrace
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public int Descriptor
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IOknaDocument Document
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool EqualClearenceOfBars
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public XElement ExtendedProperties
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public int GalleryIdx
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public uint GUID
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string GUIDString
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string IntegrationArticle
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string IntegrationColor
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsFinalProduct
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsOK
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsUpdatable
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool Modified
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public uint OriginalGUID
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string OriginalGUIDString
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerable<IBar> Outline
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool PaintInside
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool PaintOutside
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IPart Parent
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IPosition Position
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public RectangleF Rectangle
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public Dictionary<ISymbolColor, List<IPart>> Seals
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool Selected
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IArea ShapeArea
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool ShouldBeAssembled
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IFrameBase Shutter
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public ISill Sill
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public int Tag
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public ITopObject TopObject
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public EProfileType Type
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public int WeatherboardsColor
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public Dictionary<ISymbolColor, List<IPart>> Gaskets
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
#pragma warning disable 0067
            public event AfterPaintEvent AfterPaint;
            public event AfterUpdateEvent AfterUpdate;
            public event BeforePaintEvent BeforePaint;
            public event BeforeUndoEvent BeforeUndo;
            public event ResizeEvent OnResize;
#pragma warning restore 0067

            public void AddAdditionalPart(IPart value)
            {
                throw new NotImplementedException();
            }

            public void AddComponentChange(string from, string to)
            {
                throw new NotImplementedException();
            }

            public ISill AddSill(string nr_art, double width, int color, double correction, bool includesCapCorrection, string capArticle)
            {
                throw new NotImplementedException();
            }

            public void Calculate()
            {
                throw new NotImplementedException();
            }

            public bool ChangeComponents()
            {
                throw new NotImplementedException();
            }

            public bool ChangeHandle(string change)
            {
                throw new NotImplementedException();
            }

            public int[] CommonColors()
            {
                throw new NotImplementedException();
            }

            public void Copy()
            {
                throw new NotImplementedException();
            }

            public void Copy(Stream to)
            {
                throw new NotImplementedException();
            }

            public IArea DetachArea(IArea value)
            {
                throw new NotImplementedException();
            }

            public IBar DetachBar(IBar value)
            {
                throw new NotImplementedException();
            }

            public bool ExchangeSeals(ISymbolColor from, ISymbolColor to)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IPart> FindParts(EProfileType type, bool include_bars)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IPart> FindParts(EProfileType type, bool include_bars, bool include_opt_profs)
            {
                throw new NotImplementedException();
            }

            public IDrawing GetDrawing()
            {
                throw new NotImplementedException();
            }

            public string[] GetElementsList(EProfileType type, EElementsFilter filter)
            {
                throw new NotImplementedException();
            }

            public IFrameBase GetFrame(EProfileType type)
            {
                throw new NotImplementedException();
            }

            public IFrameBase GetFrame(EProfileType type, bool start_from_parent)
            {
                throw new NotImplementedException();
            }

            public int GetNumber(EProfileType type)
            {
                throw new NotImplementedException();
            }

            public float get_Radiuses(int value)
            {
                throw new NotImplementedException();
            }

            public bool Init()
            {
                throw new NotImplementedException();
            }

            public void InvalidateCalculation()
            {
                throw new NotImplementedException();
            }

            public bool Load()
            {
                throw new NotImplementedException();
            }

            public bool Load(bool initial_load)
            {
                throw new NotImplementedException();
            }

            public bool LoadCouplingProfilesSet(string setSymbol, bool enlargeConstructionArea)
            {
                throw new NotImplementedException();
            }

            public bool LoadFromGallery(int index)
            {
                throw new NotImplementedException();
            }

            public bool LoadFromGalleryWithSize(int index)
            {
                throw new NotImplementedException();
            }

            public bool LoadPricelist(int index, int color)
            {
                throw new NotImplementedException();
            }

            public void MirrorTransform()
            {
                throw new NotImplementedException();
            }

            public bool OnCommand(uint cmd)
            {
                throw new NotImplementedException();
            }

            public bool OnCommand(uint cmd, uint hint)
            {
                throw new NotImplementedException();
            }

            public bool OnCommand(uint cmd, uint hint, uint pHint)
            {
                throw new NotImplementedException();
            }

            public void Paste()
            {
                throw new NotImplementedException();
            }

            public void Paste(Stream from)
            {
                throw new NotImplementedException();
            }

            public bool PricelistValidateSize()
            {
                throw new NotImplementedException();
            }

            public double PropertyValue(int prop)
            {
                throw new NotImplementedException();
            }

            public double PropertyValue(int prop, ECalcGroup set)
            {
                throw new NotImplementedException();
            }

            public RectangleF RebateRectangle(ECorrect correction)
            {
                throw new NotImplementedException();
            }

            public void RemoveAdditionalPart(IPart value)
            {
                throw new NotImplementedException();
            }

            public void set_Radiuses(int value1, float value2)
            {
                throw new NotImplementedException();
            }

            public void Undo(string reason)
            {
                throw new NotImplementedException();
            }

            public bool ExchangeGaskets(ISymbolColor from, ISymbolColor to)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}
