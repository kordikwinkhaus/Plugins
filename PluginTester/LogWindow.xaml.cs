using System.Windows;

namespace PluginTester
{
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dynamic logger = this.DataContext;
            string text = logger.Data?.ToString() ?? string.Empty;
            txtUserData.Text = text;
        }
    }
}
