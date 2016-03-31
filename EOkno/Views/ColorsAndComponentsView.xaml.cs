using System.Windows;
using System.Windows.Controls;

namespace EOkno.Views
{
    public partial class ColorsAndComponentsView : UserControl
    {
        public ColorsAndComponentsView()
        {
            InitializeComponent();
        }

        public ScrollBarVisibility ComponentsScrollbar
        {
            get { return (ScrollBarVisibility)GetValue(ComponentsScrollbarProperty); }
            set { SetValue(ComponentsScrollbarProperty, value); }
        }

        public static readonly DependencyProperty ComponentsScrollbarProperty =
            DependencyProperty.Register("ComponentsScrollbar", typeof(ScrollBarVisibility), 
            typeof(ColorsAndComponentsView), new PropertyMetadata(ScrollBarVisibility.Disabled));
    }
}
