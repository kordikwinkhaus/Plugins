using System.Collections.ObjectModel;
using System.Windows.ViewModels;

namespace Ctor.ViewModels
{
    public class TreeViewItemViewModel : ViewModelBase
    {
        private static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren)
        {
            this.Parent = parent;

            this.Children = new ObservableCollection<TreeViewItemViewModel>();
            if (lazyLoadChildren)
            {
                this.Children.Add(DummyChild);
            }
        }

        private TreeViewItemViewModel()
        {
        }

        public ObservableCollection<TreeViewItemViewModel> Children { get; private set; }

        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        protected void AddDummyChild()
        {
            this.Children.Clear();
            this.Children.Add(DummyChild);
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && this.Parent != null)
                {
                    this.Parent.IsExpanded = true;
                }

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        bool IsSelected { get; set; }

        public TreeViewItemViewModel Parent { get; private set; }

        protected virtual void LoadChildren()
        {
        }
    }
}
