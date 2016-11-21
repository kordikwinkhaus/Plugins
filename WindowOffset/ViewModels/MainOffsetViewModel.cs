using System;
using System.Collections.Generic;
using System.Drawing;
using WindowOffset.Models;

namespace WindowOffset.ViewModels
{
    public class MainOffsetViewModel : SideOffsetViewModel
    {
        private readonly List<SideOffsetViewModel> _subitems = new List<SideOffsetViewModel>();
        private readonly MainOffset _mainModel;
        private readonly PointF _centroid;

        internal MainOffsetViewModel(MainOffset model, PointF centroid)
            : base(model)
        {
            _mainModel = model;
            _centroid = centroid;
        }

        public override int Offset
        {
            get { return base.Offset; }
            set
            {
                base.Offset = value;
                foreach (var subitem in _subitems)
                {
                    subitem.TrySetParentOffset(value);
                }
            }
        }

        internal void Add(SideOffsetViewModel subitem)
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

        public override void Recalculate(double scale, double left, double top)
        {
            this.X = _centroid.X / scale + left;
            this.Y = _centroid.Y / scale + top;
        }
    }
}
