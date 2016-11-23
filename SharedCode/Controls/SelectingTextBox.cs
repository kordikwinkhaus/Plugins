using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Okna.Plugins.Controls
{
    public class SelectingTextBox : TextBox
    {
        private bool _numeric = false;
        public bool Numeric
        {
            get { return _numeric; }
            set { _numeric = value; }
        }

        public SelectingTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(TextChangedEvent, new TextChangedEventHandler(OnTextChanged), true);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int ci = tb.CaretIndex;
            string txt = tb.Text;
            string sep = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            if (sep != "." && txt.Contains("."))
            {
                int carret = tb.CaretIndex;
                txt = txt.Replace(".", sep);
                tb.Text = txt;
                tb.CaretIndex = carret;
            }
            if (_numeric)
            {
                foreach (char c in txt)
                {
                    if ((c < '0' || c > '9') && c != sep[0])
                    {
                        tb.Text = tb.Text.Replace(new string(c, 1), "");
                    }
                }
            }
            if (tb.Text.Length > ci)
            {
                ci = tb.Text.Length;
            }
            tb.CaretIndex = ci;
        }

        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox        
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focussed, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }
    };
}
