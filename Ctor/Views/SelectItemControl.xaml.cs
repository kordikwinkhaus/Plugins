using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ctor.Views
{
    public partial class SelectItemControl : UserControl
    {
        public SelectItemControl()
        {
            InitializeComponent();
        }

        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register("DisplayText", typeof(string), typeof(SelectItemControl), new PropertyMetadata(null));

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(SelectItemControl), new PropertyMetadata(null));

        public ICommand SelectValueCommand
        {
            get { return (ICommand)GetValue(SelectValueCommandProperty); }
            set { SetValue(SelectValueCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectValueCommandProperty =
            DependencyProperty.Register("SelectValueCommand", typeof(ICommand), typeof(SelectItemControl), new PropertyMetadata(null));
    }
}
