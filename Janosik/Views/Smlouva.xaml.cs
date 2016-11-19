using System.Windows.Controls;
using System.Xml.Linq;
using UserExtensions;
using WHOkna;

namespace Janosik.Views
{
    /// <summary>
    /// Interaction logic for Smlouva.xaml
    /// </summary>
    public partial class Smlouva : UserControl, IUserForm5
    {
        public Smlouva()
        {
            InitializeComponent();
        }

        public IDataRequest DataCallback { get; set; }

        public int Index { get; set; }

        public XElement ObjectData { get; set; }

        public IOknaDocument OknaDoc { get; set; }

        public IPart Part { get; set; }

        public string Table { get; set; }

        public object Title 
        {
            get
            {
                return "Smlouva";
            }
        }

        public int DisableStandardPages()
        {
            return 1;
        }

        public bool OnDialogResult(bool ok)
        {
            return false;
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            return true;
        }

        public bool ShouldShowFor(IOknaDocument doc, string state)
        {
            return true;
        }

        public bool ValidateData()
        {
            return false;
        }
    }
}
