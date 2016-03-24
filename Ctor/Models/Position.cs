using System;
using System.Collections.Generic;
using System.Linq;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt pozice.
    /// </summary>
    public class Position
    {
        private readonly IPosition _position;

        internal Position(IPosition position)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));

            _position = position;
        }

        /// <summary>
        /// Vrací ID pozice.
        /// </summary>
        public int ID
        {
            get { return _position.Pos; }
        }

        /// <summary>
        /// Vrací/nastavuje číslo pozice.
        /// </summary>
        public string Number
        {
            get { return _position.Tag; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException();
                _position.Tag = value;
            }
        }

        /// <summary>
        /// Vrací/nastavuje popis pozice.
        /// </summary>
        public string Description
        {
            get { return _position.Description; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException();
                _position.Description = value;
            }
        }

        /// <summary>
        /// Vrací true, pokud má pozice "kreslící plochu".
        /// </summary>
        public bool HasTopObject
        {
            get { return _position.Data != null; }
        }

        /// <summary>
        /// Vrací true, pokud se jedná o pozici okna, dveří nebo posuvných dveří.
        /// </summary>
        public bool IsConstruction
        {
            get { return _position.Type == EPosType.kOkno ||
                         _position.Type == EPosType.kDrzwi ||
                         _position.Type == EPosType.kDrzwiPrzesuwne;
            }
        }

        /// <summary>
        /// Vrací šířku pozice.
        /// </summary>
        public float Width
        {
            get
            {
                CheckTopObject();
                return _position.Data.Dimensions.Width;
            }
        }

        /// <summary>
        /// Vrací výšku pozice.
        /// </summary>
        public float Height
        {
            get
            {
                CheckTopObject();
                return _position.Data.Dimensions.Height;
            }
        }

        /// <summary>
        /// Vymaže prvky v kreslící ploše.
        /// </summary>
        public void Clear()
        {
            CheckTopObject();

            ITopObject top = _position.Data;
            foreach (uint guid in top.FindParts(EProfileType.tOsciez, false).Select(f => f.GUID))
            {
                var part = _position.Document.FromGUID(guid);
                if (part != null)
                {
                    part.OnCommand(Commands.Delete);
                }
            }

            foreach (IPart part in top.FindParts(EProfileType.tPLacz, true)
                           .Concat(top.FindParts(EProfileType.tNullPLacz, true)))
            {
                part.OnCommand(Commands.Delete);
            }

            top.Invalidate();
        }

        /// <summary>
        /// Zrcadlení pozice.
        /// </summary>
        public void Mirror()
        {
            CheckTopObject();

            IFrameBase frame = _position.Data.GetFrame(EProfileType.tDziura);
            frame.MirrorTransform();
            frame.Update(false);
        }

        /// <summary>
        /// Vloží rámy do prázdných polí.
        /// </summary>
        /// <param name="type">ID typu.</param>
        /// <param name="color">ID barvy.</param>
        public void InsertFrames(int type, int color)
        {
            CheckTopObject();

            var parameters = Parameters.ForFrameType(type, color);
            foreach (IArea area in _position.Data.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tOsciez, parameters);
            }
        }

        /// <summary>
        /// Vloží rám do prázdného pole, které obsahuje předaný bod.
        /// </summary>
        /// <param name="type">ID typu.</param>
        /// <param name="color">ID barvy.</param>
        /// <param name="pointInAreaX">X-ová souřadnice bodu pro výběr pole.</param>
        /// <param name="pointInAreaY">Y-ová souřadnice bodu pro výběr pole.</param>
        public Frame InsertFrame(int type, int color, float pointInAreaX, float pointInAreaY)
        {
            CheckTopObject();

            foreach (IArea area in _position.Data.Areas.Where(a => a.Child == null))
            {
                if (area.Rectangle.Contains(pointInAreaX, pointInAreaY))
                {
                    var parameters = Parameters.ForFrameType(type, color);
                    area.AddChild(EProfileType.tOsciez, parameters);

                    IFrame frame = area.FindFrame();
                    if (frame != null)
                    {
                        return new Frame(frame);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Vloží rám do prázdného pole. Pokud pozice obsahuje více prázdných polí, 
        /// zobrazí dialog pro výběr pole.
        /// </summary>
        /// <param name="type">ID typu.</param>
        /// <param name="color">ID barvy.</param>
        public Frame InsertFrame(int type, int color)
        {
            CheckTopObject();

            var areas = _position.Data.Areas.Where(a => a.Child == null).ToList();

            var barvy = _position.Data.CommonColors();

            if (areas.Count == 1)
            {
                var area = areas[0];
                var parameters = Parameters.ForFrameType(type, color);
                area.AddChild(EProfileType.tOsciez, parameters);

                IFrame frame = area.FindFrame();
                if (frame != null)
                {
                    return new Frame(frame);
                }
            }
            else
            {
                // TODO: zobrazit dialog
            }

            return null;
        }

        /// <summary>
        /// Vrací rám pro zadaný index.
        /// </summary>
        /// <param name="id">Index rámu.</param>
        public Frame this[int id]
        {
            get { return GetFrame(id); }
        }

        /// <summary>
        /// Vrací rám pro zadaný index.
        /// </summary>
        /// <param name="id">Index rámu.</param>
        public Frame GetFrame(int id)
        {
            CheckTopObject();

            foreach (IFrame frame in _position.Data.FindParts(EProfileType.tOsciez, false))
            {
                int number = frame.GetNumber(EProfileType.tOsciez);
                if (id == number)
                {
                    return new Frame(frame);
                }
            }

            return null;
        }

        /// <summary>
        /// Vrací všechny rámy v konstrukci.
        /// </summary>
        public IEnumerable<Frame> GetFrames()
        {
            CheckTopObject();

            foreach (IFrame frame in _position.Data.FindParts(EProfileType.tOsciez, false))
            {
                yield return new Frame(frame);
            }
        }

        /// <summary>
        /// Vloží zadaný paket do všech prázdných polí (rámy a křídla) v pozici.
        /// </summary>
        /// <param name="nrArt">Číslo výrobku paketu.</param>
        public void InsertGlasspackets(string nrArt)
        {
            var parameters = Parameters.ForGlasspacket(nrArt);
            foreach (var frame in GetFrames())
            {
                frame.InsertGlasspackets(parameters);
            }
        }

        private void CheckTopObject()
        {
            if (_position.Data == null) throw new ModelException("This position doesn't have top object.");
        }

        public void Msg(object o)
        {
            string msg = o?.ToString() ?? "NULL";
            System.Windows.MessageBox.Show(msg);
        }

        public void Areas(int type, int color)
        {
            var parameters = Parameters.ForFrameType(type, color);
            // TODO: area lze dostat přes Rectangle
            foreach (var a in _position.Data.Areas)
            {
                if (a.Rectangle.X > 400 & a.Rectangle.Y > 400)
                {
                    a.AddChild(EProfileType.tOsciez, parameters);

                }
            }
        }
    }
}
