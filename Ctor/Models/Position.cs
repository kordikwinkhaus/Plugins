using System;
using System.Collections.Generic;
using System.Linq;
using WHOkna;

namespace Ctor.Models
{
    public class Position
    {
        private readonly IPosition _position;

        internal Position(IPosition position)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));

            _position = position;
        }

        public void Clear()
        {
            ITopObject top = _position.Data;
            foreach (uint guid in top.FindParts(EProfileType.tOsciez, false).Select(f => f.GUID))
            {
                var f = _position.Document.FromGUID(guid);
                if (f != null)
                {
                    f.OnCommand(Commands.Delete);
                }
            }

            foreach (IPart p in top.FindParts(EProfileType.tPLacz, true)
                        .Concat(top.FindParts(EProfileType.tNullPLacz, true)))
            {
                p.OnCommand(Commands.Delete);
            }

            top.Invalidate();
        }

        public void InsertFrames(int type, int color)
        {
            var parameters = Parameters.ForFrameType(type, color);

            foreach (IArea area in _position.Data.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tOsciez, parameters);
            }
        }

        public Frame this[int id]
        {
            get { return GetFrame(id); }
        }

        public Frame GetFrame(int id)
        {
            foreach (IFrameBase frame in _position.Data.FindParts(EProfileType.tOsciez, false))
            {
                int number = frame.GetNumber(EProfileType.tOsciez);
                if (id == number)
                {
                    return new Frame(frame);
                }
            }

            return null;
        }

        public IEnumerable<Frame> GetFrames()
        {
            foreach (IFrameBase frame in _position.Data.FindParts(EProfileType.tOsciez, false))
            {
                yield return new Frame(frame);
            }
        }

        public void InsertGlasspackets(string nrArt)
        {
            var parameters = Parameters.ForGlasspacket(nrArt);
            foreach (var frame in GetFrames())
            {
                frame.InsertGlasspackets(parameters);
            }
        }
    }
}
