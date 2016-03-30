using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Okna.Plugins.ViewModels;

namespace Ctor.ViewModels
{
    public class ContextViewModel : ViewModelBase
    {
        private readonly FastInsertViewModel _parent;

        internal ContextViewModel(FastInsertViewModel parent)
        {
            _parent = parent;

            this.SelectWindowTypeCommand = new RelayCommand(SelectWindowType);
            this.SelectWindowColorCommand = new RelayCommand(SelectWindowColor);
        }

        #region Windows type

        private int _windowTypeID;
        public int WindowTypeID
        {
            get { return _windowTypeID; }
            set
            {
                if (_windowTypeID != value)
                {
                    _windowTypeID = value;
                    OnPropertyChanged(nameof(WindowTypeID));
                }
            }
        }

        private string _windowTypeName;
        public string WindowTypeName
        {
            get { return _windowTypeName; }
            set
            {
                if (_windowTypeName != value)
                {
                    _windowTypeName = value;
                    OnPropertyChanged(nameof(WindowTypeName));
                }
            }
        }

        public ICommand SelectWindowTypeCommand { get; private set; }

        private void SelectWindowType(object param)
        {
            var dealer = _parent.Document.Dealer;
            var app = _parent.Application;
            var elementFilter = app.HideAtDealerFilter("typyp", dealer, "e.");
            var groupFilter = app.HideAtDealerFilter("gruptech", dealer, "g.");

            var dialog = _parent.DialogFactory.GetSearchDialog("SelectType", elementFilter, groupFilter);
            if (dialog.ShowDialog(app.MainWindowHWND()) == true)
            {
                this.WindowTypeName = (string)dialog.SelectedText;
                this.WindowTypeID = (int)dialog.Selected;
            }
        }

        #endregion

        #region Windows color

        private int _windowColorID;
        public int WindowColorID
        {
            get { return _windowColorID; }
            set
            {
                if (_windowColorID != value)
                {
                    _windowColorID = value;
                    OnPropertyChanged(nameof(WindowColorID));
                }
            }
        }

        private string _windowColorName;
        public string WindowColorName
        {
            get { return _windowColorName; }
            set
            {
                if (_windowColorName != value)
                {
                    _windowColorName = value;
                    OnPropertyChanged(nameof(WindowColorName));
                }
            }
        }

        public ICommand SelectWindowColorCommand { get; private set; }

        private void SelectWindowColor(object param)
        {

        }

        #endregion
    }
}
