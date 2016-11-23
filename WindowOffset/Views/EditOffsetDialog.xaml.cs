using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using WindowOffset.ViewModels;

namespace WindowOffset.Views
{
    public partial class EditOffsetDialog : Window
    {
        public EditOffsetDialog()
        {
            InitializeComponent();
        }

        private EditOffsetViewModel _viewmodel;
        internal EditOffsetViewModel ViewModel
        {
            get { return _viewmodel; }
            set { this.DataContext = _viewmodel = value; }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_viewmodel != null)
            {
                _viewmodel.RecalculateSize(windowArea.ActualWidth, windowArea.ActualHeight, GetTextHeight());
            }
        }

        public virtual bool ShowDialog(IntPtr hWndParent)
        {
            if (hWndParent != IntPtr.Zero)
            {
                WindowInteropHelper helper = new WindowInteropHelper(this);
                helper.Owner = hWndParent;
            }

            return this.ShowDialog() == true;
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                bool shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

                FocusNextOffset(e, shiftPressed);
            }
            else if (e.Key == Key.Down)
            {
                FocusNextOffset(e, false);
            }
            else if (e.Key == Key.Up)
            {
                FocusNextOffset(e, true);
            }
        }
        private void FocusNextOffset(KeyEventArgs e, bool reversed)
        {
            int index = grid.SelectedIndex;
            int newIndex = (reversed) ? index - 1 : index + 1;

            if (newIndex < 0)
            {
                if (reversed)
                {
                    // TODO: vybrat textbox na canvasu?
                    e.Handled = true;
                }
            }
            else if (newIndex < grid.Items.Count)
            {
                FocusTextBoxInRow(e, newIndex);
            }
            else
            {
                // TODO: vybrat textbox na canvasu?
                e.Handled = true;
            }
        }

        private void FocusTextBoxInRow(KeyEventArgs e, int newIndex)
        {
            // vybrat nový řádek
            DataGridRow newRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(newIndex);

            TextBox txt = FindTextBox(newRow);
            if (txt != null)
            {
                e.Handled = true;
                grid.SelectedIndex = newIndex;
                txt.Focus();
            }
        }

        private TextBox FindTextBox(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject obj = VisualTreeHelper.GetChild(parent, i);
                if (obj is TextBox)
                {
                    return (TextBox)obj;
                }
                TextBox result = FindTextBox(obj);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private double _textSize = 0;
        private double GetTextHeight()
        {
            if (_textSize == 0)
            {
                var formattedText = new FormattedText("1234567890",
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(tbFontSettings.FontFamily, tbFontSettings.FontStyle, tbFontSettings.FontWeight, tbFontSettings.FontStretch),
                    tbFontSettings.FontSize,
                    Brushes.Black);
                _textSize = formattedText.Height;
            }

            return _textSize;
        }
    }
}
