using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindowOffset.Views
{
    public partial class EditOffsetDialog : Window
    {
        public EditOffsetDialog()
        {
            InitializeComponent();
        }

        public virtual bool ShowDialog(IntPtr hWndParent)
        {
            if (hWndParent != IntPtr.Zero)
            {
                WindowInteropHelper helper = new WindowInteropHelper(this);
                helper.Owner = hWndParent;
            }

            return this.ShowDialog() == true;
        }
    }
}
