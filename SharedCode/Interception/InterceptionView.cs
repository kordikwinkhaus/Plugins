using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using UserExtensions;

namespace Okna.Plugins.Interception
{
    public class InterceptionView : UserControl, IUserForm
    {
        protected readonly Logger _logger;
        private Window _logWindow;
        private TextBox _txtUserData;

        public InterceptionView(IUserForm userform, bool documentView = false)
        {
            if (userform == null) throw new ArgumentNullException(nameof(userform));

            _logger = new Logger();
            this.UserForm = userform;

            BuildUI(documentView);
        }

        private void BuildUI(bool documentView)
        {
            this.SnapsToDevicePixels = true;
            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);

            Grid grid = new Grid();
            if (documentView)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            else
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            this.AddChild(grid);

            var uf = (FrameworkElement)this.UserForm;
            uf.AddHandler(MouseUpEvent, new MouseButtonEventHandler(testContainer_MouseUp), true);
            uf.AddHandler(KeyUpEvent, new KeyEventHandler(testContainer_KeyUp), true);
            grid.Children.Add(uf);

            DockPanel cell2 = new DockPanel();
            Button btnShowInterceptor = new Button { Content = "Show interceptor" };
            DockPanel.SetDock(btnShowInterceptor, Dock.Bottom);
            btnShowInterceptor.Click += showInterceptor_Click;
            cell2.Children.Add(btnShowInterceptor);

            _txtUserData = new TextBox();
            _txtUserData.TextWrapping = TextWrapping.Wrap;
            _txtUserData.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Grid.SetRow(_txtUserData, 2);
            cell2.Children.Add(_txtUserData);

            if (documentView)
            {
                cell2.Width = 250;
                Grid.SetColumn(cell2, 1);
            }
            else
            {
                _txtUserData.Height = 200;
                Grid.SetRow(cell2, 1);
            }

            grid.Children.Add(cell2);
        }

        private void testContainer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RefreshXmlText();
        }

        private void testContainer_KeyUp(object sender, KeyEventArgs e)
        {
            RefreshXmlText();
        }

        private void RefreshXmlText()
        {
            _txtUserData.Text = _logger.Data?.ToString() ?? string.Empty;
        }

        private void showInterceptor_Click(object sender, RoutedEventArgs e)
        {
            if (_logWindow == null)
            {
                try
                {
                    var directory = Utils.GetPluginDirectory<Logger>();
                    string assemblyFilename = System.IO.Path.Combine(directory, "PluginTester.exe");
                    Assembly asm = Assembly.LoadFile(assemblyFilename);
                    var logWinType = asm.GetType("PluginTester.LogWindow");
                    _logWindow = (Window)Activator.CreateInstance(logWinType);
                    _logWindow.DataContext = _logger;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Nelze vytvořit log window", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            _logWindow.Show();
        }

        public IUserForm UserForm { get; private set; }

        public XElement ObjectData
        {
            get
            {
                var ev = _logger.Log(nameof(InterceptionView), "get_" + nameof(ObjectData));
                XElement returnValue = this.UserForm.ObjectData;
                ev.ReturnValue = returnValue;
                return returnValue;
            }
            set
            {
                _logger.Log(nameof(InterceptionView), "set_" + nameof(ObjectData), value);
                this.UserForm.ObjectData = value;
            }
        }

        public object Title
        {
            get { return this.UserForm.Title; }
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            _logger.Data = data;
            RefreshXmlText();

            var ev = _logger.Log(nameof(InterceptionView), nameof(SetData), data, document, position, profileType);
            bool returnValue = this.UserForm.SetData(data, document, position, profileType);
            ev.ReturnValue = returnValue;
            return returnValue;
        }
    }
}
