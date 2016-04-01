using Ctor.Models;
using Ctor.Resources;

namespace Ctor.ViewModels
{
    public class Context : IContext
    {
        private ContextViewModel _parent;

        internal Context(ContextViewModel parent)
        {
            _parent = parent;
        }

        public int WindowsType
        {
            get
            {
                int id = _parent.WindowTypeID;
                if (id == 0)
                {
                    throw new ModelException(Strings.SelectWindowTypeFirst);
                }
                return id;
            }
        }

        public int WindowsColor
        {
            get
            {
                int id = _parent.WindowColorID;
                if (id == 0)
                {
                    throw new ModelException(Strings.SelectWindowColorFirst);
                }
                return id;
            }
        }

        public string Glasspacket
        {
            get
            {
                string nr_art = null;
                if (_parent.UseDefaultGlasspacket)
                {
                    int winType = this.WindowsType; // throws exception if nothing selected
                    nr_art = _parent.WindowType.DefaultGlasspacket;
                }
                else
                {
                    nr_art = _parent.GlasspacketNrArt;
                }

                if (string.IsNullOrEmpty(nr_art))
                {
                    throw new ModelException(Strings.SelectGlasspacketFirst);
                }

                return nr_art;
            }
        }

        public bool InsertFittings
        {
            get { return _parent.InsertFittings; }
        }

        public int FittingsGroup
        {
            get
            {
                if (_parent.UseDefaultFittingsGroup)
                {
                    int winType = this.WindowsType; // throws exception if nothing selected
                    return _parent.WindowType.DefaultFittingsGroup;
                }
                else
                {
                    int id = _parent.FittingsGroupID;
                    if (id == 0)
                    {
                        throw new ModelException(Strings.SelectFittingsGroupFirst);
                    }
                    return id;
                }
            }
        }
    }
}