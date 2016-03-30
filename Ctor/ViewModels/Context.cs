using Ctor.Models;

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
                    throw new ModelException("Vyberte nejdříve typ okna.");
                }
                return id;
            }
        }

        public int WindowsColor
        {
            get { return 253; }
        }
    }
}