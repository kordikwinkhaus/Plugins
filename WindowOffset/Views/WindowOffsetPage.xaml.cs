﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using UserExtensions;
using WHOkna;
using WindowOffset.ViewModels;

namespace WindowOffset.Views
{
    public partial class WindowOffsetPage : UserControl, IUserForm3, INotifyPropertyChanged
    {
        private XElement _data;
        private ITopObject _topObject;

        public WindowOffsetPage()
        {
            InitializeComponent();
        }

        public object Title
        {
            get { return Properties.Resources.PluginTitle; }
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

        private IOknaDocument _oknaDoc;
        public IOknaDocument OknaDoc
        {
            get { return _oknaDoc; }
            set { _oknaDoc = value; }
        }

        public IDataRequest DataCallback { get; set; }

        private IPart _part;
        public IPart Part
        {
            get { return _part; }
            set
            {
                _part = value;
                _topObject = value as ITopObject;
                this.cmdEdit.IsEnabled = CanEditOffset();
            }
        }

        private bool CanEditOffset()
        {
            if (_topObject == null) return false;
            if (_topObject.Position == null) return false;

            var type = _topObject.Position.Type;
            bool typeOk = type == EPosType.kOkno || 
                          type == EPosType.kDrzwi || 
                          type == EPosType.kDrzwiPrzesuwne;

            if (!typeOk) return false;

            for (int i = 0; i < 3; i++)
            {
                if (_topObject.get_Radiuses(i) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            if (data == null) return false;
            if (_data == data) return true;

            _data = data;

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

        private void UserControl_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // znovu otestovat - mohlo dojít k editaci pozice
                if (!CanEditOffset())
                {
                    MessageBox.Show(Properties.Resources.CannotEditOffset, Properties.Resources.PluginTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; 
                }

                var dlg = new EditOffsetDialog();
                var vm = new EditOffsetViewModel(_data, _topObject);
                dlg.ViewModel = vm;
            
                if (dlg.ShowDialog(this.OknaDoc?.Application?.MainWindowHWND() ?? IntPtr.Zero) && vm.Failed)
                {
                    MessageBox.Show(Properties.Resources.EditOffsetFailed, Properties.Resources.PluginTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                WinkhausCR.Bugs.Logger.Instance.Log(ex);
            }
        }
    }
}
