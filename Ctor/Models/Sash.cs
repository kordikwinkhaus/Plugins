using System;
using System.Collections.Generic;
using System.Linq;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Objekt křídla.
    /// </summary>
    public class Sash
    {
        private readonly ISash _sash;
        private readonly Frame _parent;

        internal Sash(ISash sash, Frame parent)
        {
            if (sash == null) throw new ArgumentNullException(nameof(sash));
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _sash = sash;
            _parent = parent;
        }

        /// <summary>
        /// Vrací rám, ve kterém je toto křídlo.
        /// </summary>
        public Frame Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Vrací ID křídla (pořadová čísla od jedné).
        /// </summary>
        public int ID
        {
            get { return _sash.GetNumber(EProfileType.tSkrz); }
        }

        /// <summary>
        /// Vloží zadaný paket do prázdných polí křídla.
        /// </summary>
        /// <param name="nrArt">Číslo výrobku paketu.</param>
        public void InsertGlasspackets(string nrArt)
        {
            InsertGlasspackets(Parameters.ForGlasspacket(nrArt));
        }

        internal void InsertGlasspackets(Dictionary<string, object> parameters)
        {
            foreach (IArea area in _sash.Areas.Where(a => a.Child == null))
            {
                area.AddChild(EProfileType.tSzyba, parameters);
            }
        }

        /// <summary>
        /// Vloží kování do křídla.
        /// </summary>
        /// <param name="type">ID typu kování.</param>
        public void InsertFittings(int type)
        {
            this.InsertFittingsCore(type, null);
        }

        /// <summary>
        /// Vloží kování do křídla.
        /// </summary>
        /// <param name="type">ID typu kování.</param>
        /// <param name="inversion">Zda-li použít inverzi.</param>
        public void InsertFittings(int type, bool inversion)
        {
            this.InsertFittingsCore(type, inversion);
        }

        private void InsertFittingsCore(int type, bool? inversion)
        {
            var doc = _sash.TopObject.Position.Document;
            bool oldUpdateTechnology = doc.UpdateTechnology;
            int oldFittingsType = _sash.FittingsType;
            bool oldFittingsInversion = _sash.FittingsInversion;

            try
            {
                doc.UpdateTechnology = true;

                _sash.FittingsType = type;
                // ISash.FittingsInversionEnabled není implementováno
                //if (_sash.FittingsInversionEnabled)
                //{
                if (inversion.HasValue)
                {
                    _sash.FittingsInversion = inversion.Value;
                }
                //}

                if (_sash.Update(true))
                {
                    _sash.TopObject.CheckPoint();
                }
                else
                {
                    _sash.FittingsType = oldFittingsType;
                    _sash.FittingsInversion = oldFittingsInversion;
                    _sash.Update(true);
                }
            }
            finally
            {
                doc.UpdateTechnology = oldUpdateTechnology;
            }
        }

        /// <summary>
        /// Vrací parametry pro vyhledání výběru kování.
        /// </summary>
        public FittingsFindArgs GetFittingsFindArgs()
        {
            FittingsFindArgs result = new FittingsFindArgs
            {
                WindowTypeID = _parent.WindowTypeID
            };

            var falseMullion = _parent.FindFalseMullion(_sash);
            if (falseMullion != null)
            {
                float midpointX = falseMullion.Offset.X;
                float distLeftEdge = Math.Abs(midpointX - _sash.Rectangle.Left);
                float distRightEdge = Math.Abs(midpointX - _sash.Rectangle.Right);

                EMountSide sideToCompare = EMountSide.msRight;
                if (distLeftEdge < distRightEdge)
                {
                    // štulp je zleva
                    sideToCompare = EMountSide.msLeft;
                }

                if (falseMullion.MountSide == sideToCompare)
                {
                    // horní křídlo
                    result.FalseMullion1 = true;
                }
                else
                {
                    // dolní křídlo
                    result.FalseMullion2 = true;
                    result.FalseMullionNrArt = falseMullion.Article;
                }
            }
            else
            {
                result.Standard = true;
            }

            return result;
        }
    }
}
