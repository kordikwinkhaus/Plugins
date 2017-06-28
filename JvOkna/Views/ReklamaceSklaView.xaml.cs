using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using JvOkna.ViewModels;
using UserExtensions;

namespace JvOkna.Views
{
    public partial class ReklamaceSklaView : UserControl, IUserForm, INotifyPropertyChanged
    {
        private ReklamaceSklaViewModel _viewmodel;

        public ReklamaceSklaView()
        {
            InitializeComponent();
        }

        private XElement _objectData;
        public XElement ObjectData
        {
            get { return _objectData; }
            set
            {
                _objectData = value;
                NotifyPropertyChanged();
            }
        }

        public object Title
        {
            get { return "Reklamace"; }
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            if (data == null) return false;

            var pluginElement = data.Element(Xml.ReklamaceSkla);
            if (pluginElement == null)
            {
                pluginElement = new XElement(Xml.ReklamaceSkla);
                data.Add(pluginElement);
            }

            _viewmodel.Init(pluginElement);

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewmodel = (ReklamaceSklaViewModel)e.NewValue;
        }

        private void UserControl_PreviewMouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
