using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Imager.ViewModels;
using UserExtensions;
using WHOkna;

namespace Imager.Views
{
    public partial class ImagerPage : UserControl, IUserForm3
    {
        private readonly ImagerViewModel _viewmodel;

        public ImagerPage()
        {
            InitializeComponent();

            this.DataContext = _viewmodel = new ImagerViewModel();
        }

        public IDataRequest DataCallback { get; set; }

        public XElement ObjectData { get; set; }

        public IOknaDocument OknaDoc { get; set; }

        private IPart _part;
        public IPart Part
        {
            get { return _part; }
            set
            {
                _part = value;
                _viewmodel.Part = value;
            }
        }

        public object Title
        {
            get { return "Imager"; }
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            return true;
        }

        private void UserControl_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
