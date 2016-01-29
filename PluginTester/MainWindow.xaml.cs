using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using UserExtensions;

namespace PluginTester
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length < 3) Environment.Exit(1);

            string dllName = args[1];
            if (!Path.IsPathRooted(dllName))
            {
                dllName = Path.Combine(Directory.GetCurrentDirectory(), dllName);
            }
            txtConnString.Text = args[2];

            IExtensionsFactory fact = GetFactory(dllName);
            if (fact != null)
            {
                CreatePages(fact);
            }
        }

        private IExtensionsFactory GetFactory(string dllName)
        {
            try
            {
                Assembly asm = Assembly.LoadFile(dllName);
                var factType = asm.GetType("UserExt.ExtensionsFactory");
                IExtensionsFactory fact = (IExtensionsFactory)Activator.CreateInstance(factType);
                return fact;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot load tested dll: " + ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void CreatePages(IExtensionsFactory fact)
        {
            var vals = Enum.GetValues(typeof(EPropPage));

            foreach (var val in vals)
            {
                try
                {
                    FrameworkElement page = fact.GetPropertyPage((EPropPage)val, txtConnString.Text, CultureInfo.CurrentUICulture.Name);
                    if (page != null)
                    {
                        ListBoxItem item = new ListBoxItem();
                        item.Content = page.GetType().Name;
                        item.Tag = page;

                        lstPages.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Cannot get page for {0}: ", val) + ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lstPages.SelectedItem != null)
            {
                ListBoxItem item = (ListBoxItem)lstPages.SelectedItem;

                TestWindow win = item.Tag as TestWindow;
                if (win != null)
                {
                    if (win.IsVisible)
                    {
                        win.Activate();
                    }
                    else
                    {
                        win.Show();
                    }
                }
                else
                {
                    FrameworkElement page = (FrameworkElement)item.Tag;
                    win = new TestWindow();
                    win.SetTestPage(page);
                    win.Owner = this;
                    item.Tag = win;
                    win.Show();
                }
            }
        }
    }
}
