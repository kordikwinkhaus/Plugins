using Ctor.Models;

namespace Ctor.ViewModels
{
    public class Context : IContext
    {
        private FastInsertViewModel _fastInsertViewModel;

        internal Context(FastInsertViewModel fastInsertViewModel)
        {
            _fastInsertViewModel = fastInsertViewModel;
        }

        public int Type
        {
            get { return 109; }
        }

        public int Color
        {
            get { return 253; }
        }
    }
}