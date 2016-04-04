using System.Collections.Generic;
using System.Linq;
using Ctor.Resources;
using Okna.Data;

namespace Ctor.Models
{
    /// <summary>
    /// Parametry pro vyhledání sloupku/sloupků.
    /// </summary>
    public class MullionFindArgs
    {
        private readonly string[] _suffixes = new [] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e" };
        private readonly DynamicDictionary _windowType;
        private MullionTargetType _target;
        private bool _onlyHorizontal;
        private bool _onlyVertical;

        internal MullionFindArgs(DynamicDictionary windowType)
        {
            _windowType = windowType;
        }

        /// <summary>
        /// Vyhledá sloupky pouze do rámu.
        /// </summary>
        public MullionFindArgs Frame
        {
            get
            {
                _target = MullionTargetType.Frame;
                return this;
            }
        }

        /// <summary>
        /// Vyhledá sloupky pouze do křídla.
        /// </summary>
        public MullionFindArgs Sash
        {
            get
            {
                _target = MullionTargetType.Sash;
                return this;
            }
        }

        /// <summary>
        /// Vyhledá sloupky pouze horizontální.
        /// </summary>
        public MullionFindArgs Horizontal
        {
            get
            {
                _onlyHorizontal = true;
                return this;
            }
        }

        /// <summary>
        /// Vyhledá sloupky pouze vertikální.
        /// </summary>
        public MullionFindArgs Vertical
        {
            get
            {
                _onlyVertical = true;
                return this;
            }
        }

        /// <summary>
        /// Vrací první sloupek vyhovující parametrům.
        /// </summary>
        public string Default
        {
            get
            {
                string nrArt = this.All.FirstOrDefault();
                if (string.IsNullOrEmpty(nrArt)) throw new ModelException(Strings.CannotFindMullionNrArt);
                return nrArt;
            }
        }

        /// <summary>
        /// Vrací všechny sloupky vyhovující parametrům.
        /// </summary>
        public IList<string> All
        {
            get
            {
                IEnumerable<Mullion> mullions = GetAllMullions();

                if (_onlyHorizontal && _onlyVertical)
                {
                    _onlyVertical = _onlyHorizontal = false;
                }
                if (_onlyHorizontal)
                {
                    mullions = mullions.Where(m => m.Args[1] != '2');
                }
                else if (_onlyVertical)
                {
                    mullions = mullions.Where(m => m.Args[1] != '1');
                }

                switch (_target)
                {
                    case MullionTargetType.Frame:
                        mullions = mullions.Where(m => m.Args[0] != '2');
                        break;

                    case MullionTargetType.Sash:
                        mullions = mullions.Where(m => m.Args[0] != '1');
                        break;
                }

                return mullions.Select(m => m.NrArt).ToList();
            }
        }

        private List<Mullion> GetAllMullions()
        {
            List<Mullion> mullions = new List<Mullion>();
            foreach (var suffix in _suffixes)
            {
                string nrArt = (string)_windowType.GetValue("slupek" + suffix);
                string args = (string)_windowType.GetValue("slup_ogr" + suffix);

                if (!string.IsNullOrEmpty(nrArt))
                {
                    if (string.IsNullOrEmpty(args)) args = "  ";
                    mullions.Add(new Mullion { NrArt = nrArt, Args = args });
                }
            }

            return mullions;
        }

        #region Nested classes

        private enum MullionTargetType
        {
            All, Frame, Sash
        }

        private class Mullion
        {
            internal string NrArt;
            internal string Args;
        }

        #endregion
    }
}
