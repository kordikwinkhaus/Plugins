using System.Runtime.CompilerServices;
﻿using System.ComponentModel;
using System.Windows.Controls;
using System.Xml.Linq;
using EOkno.Models;
using EOkno.ViewModels;
using UserExtensions;
using WHOkna;

namespace EOkno.Views
{
    public partial class PositionView : UserControl, IUserForm3, INotifyPropertyChanged
    {
        private PositionViewModel _viewmodel;
        private XElement _data;

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
            get { return "eOkno - komponenty"; }
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
            if (_data == data) return true;

            _data = data;

            bool created = false;
            if (data.Element(Xml.EOkno) == null)
            {
                data.SetElementValue(Xml.EOkno, string.Empty);
                created = true;
            }

            PositionData model = new PositionData(ObjectData = data.Element(Xml.EOkno));
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

        private void UserControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            _viewmodel = (PositionViewModel)e.NewValue;
        }
    }
}
