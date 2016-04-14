using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using EOkno.Models;
using EOkno.ViewModels;
using UserExtensions;

namespace EOkno.Views
{
    public partial class DocumentView : UserControl, IUserForm, INotifyPropertyChanged
    {
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
            if (data.Element(Xml.EOkno) == null)
            {
                data.SetElementValue(Xml.EOkno, string.Empty);
                created = true;
            }

            DocumentData model = new DocumentData(ObjectData = data.Element(Xml.EOkno));
            _viewmodel.SetModel(model);
            if (created)
            {
                _viewmodel.SetDefaults();
            }

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
