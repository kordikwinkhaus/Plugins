using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using EOkno.ViewModels;
using UserExtensions;

namespace EOkno.Views
{
    public partial class DocumentView : UserControl, IUserForm, INotifyPropertyChanged
    {
        private const string s_MainElement = "EOkno";

        private DocumentViewModel _viewmodel;

        public DocumentView()
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
            get { return "EOkno"; }
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            if (data == null) return false;

            bool created = false;
            if (data.Element(s_MainElement) == null)
            {
                data.SetElementValue(s_MainElement, string.Empty);
                created = true;
            }

            _viewmodel.SetMainElement(ObjectData = data.Element(s_MainElement), created);

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
            _viewmodel = (DocumentViewModel)e.NewValue;
        }
    }
}
