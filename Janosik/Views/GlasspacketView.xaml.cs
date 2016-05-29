using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Janosik.Models;
using Janosik.ViewModels;
using UserExtensions;

namespace Janosik.Views
{
    public partial class GlasspacketView : UserControl, IUserForm, INotifyPropertyChanged
    {
        private GlasspacketViewModel _viewmodel;
        private XElement _data;

        public GlasspacketView()
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
            get { return "Přesah"; }
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            if (data == null) return false;
            if (_data == data) return true;

            _data = data;

            if (data.Element(Xml.Presah) == null)
            {
                data.SetElementValue(Xml.Presah, string.Empty);
            }

            GlasspacketModel model = new GlasspacketModel(ObjectData = data.Element(Xml.Presah));
            _viewmodel.SetModel(model);

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

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)(sender)).SelectAll();
        }

        private void TextBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ((TextBox)(sender)).SelectAll();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewmodel = (GlasspacketViewModel)e.NewValue;
        }
    }
}
