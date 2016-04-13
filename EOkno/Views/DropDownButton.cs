using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace EOkno.Views
{
    public class DropDownButton : ToggleButton
    {
        public DropDownButton()
        {
            Binding binding = new Binding("DropDown.IsOpen");
            binding.Source = this;
            this.SetBinding(IsCheckedProperty, binding);
        }

        public static readonly DependencyProperty DropDownProperty =
            DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(DropDownButton), new UIPropertyMetadata(null));

        public ContextMenu DropDown
        {
            get { return (ContextMenu)GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }

        protected override void OnClick()
        {
            if (DropDown != null)
            {
                DropDown.PlacementTarget = this;
                DropDown.Placement = PlacementMode.Bottom;

                DropDown.IsOpen = true;
            }
        }
    }
}
