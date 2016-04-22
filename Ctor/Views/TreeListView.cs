using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Ctor.Views
{
    public class TreeListView : TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        private GridViewColumnCollection _columns;
        public GridViewColumnCollection Columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = new GridViewColumnCollection();
                }
                return _columns;
            }
        }
    }
}
