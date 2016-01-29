using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using UserExtensions;

namespace PluginTester
{
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();

            testContainer.AddHandler(MouseUpEvent, new MouseButtonEventHandler(testContainer_MouseUp), true);
            testContainer.AddHandler(KeyUpEvent, new KeyEventHandler(testContainer_KeyUp), true);
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
            if (!txtUserData.IsFocused)
            {
                txtUserData.Text = _userdata.ToString();
            }
            UpdateObjectDataText();
        }

        private void UpdateObjectDataText()
        {
            if (_userform != null && _userform.ObjectData != null)
            {
                txtObjectData.Text = _userform.ObjectData.ToString();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private XElement _userdata;
        private IUserForm _userform;

        public void SetTestPage(FrameworkElement elem)
        {
            this.Title += elem.GetType().Name;

            _userdata = new XElement("UserData");
            testContainer.Children.Add(elem);

            _userform = elem as IUserForm;
            if (_userform != null)
            {
                _userform.SetData(_userdata, 1, 1, 1);
            }

            RefreshXmlText();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_userform != null)
            {
                try
                {
                    TrySetData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TrySetData()
        {
            XElement userdata = XElement.Parse(txtUserData.Text);
            _userform.SetData(userdata, 1, 1, 1);
            _userdata = userdata;

            RefreshXmlText();
        }
    }
}
