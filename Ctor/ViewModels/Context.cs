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
                    throw new ModelException("select color dude!");
                }
                return id;
            }
        }
    }
}