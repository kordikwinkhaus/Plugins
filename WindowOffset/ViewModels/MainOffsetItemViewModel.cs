using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowOffset.ViewModels
{
    public class MainOffsetItemViewModel : OffsetItemViewModel
    {
        private readonly List<OffsetItemViewModel> _subitems = new List<OffsetItemViewModel>();

        public MainOffsetItemViewModel()
        {

        }

        public override int Offset
        {
            get { return base.Offset; }
            set
            {
                base.Offset = value;
                foreach (var subitem in _subitems)
                {
                    subitem.TrySetParentValue(value);
                }
            }
        }

        internal void Add(OffsetItemViewModel subitem)
        {
            if (subitem == null) throw new ArgumentNullException(nameof(subitem));

            _subitems.Add(subitem);
        }

        protected override bool CanResetValue(object param)
        {
            return this.Offset != 0;
        }

        protected override void ResetValue(object param)
        {
            this.Offset = 0;
        }
    }
}
