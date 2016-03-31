using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Xml.Linq;
using EOkno.ViewModels;
using UserExtensions;
using WHOkna;

namespace EOkno.Views
{
    public partial class PositionView : UserControl, IUserForm3, INotifyPropertyChanged
    {
        private const string s_MainElement = "EOkno";

        private PositionViewModel _viewmodel;

        public PositionView()
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

        public IPart Part { get; set; }

        private IOknaDocument _oknaDoc;
        public IOknaDocument OknaDoc 
        {
            get { return _oknaDoc; }
            set
            {
                _oknaDoc = value;
                _viewmodel.OknaDocument = value;
            }
        }

        public IDataRequest DataCallback { get; set; }

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

        private void UserControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            _viewmodel = (PositionViewModel)e.NewValue;
        }
    }
}
