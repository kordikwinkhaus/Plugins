using System.Windows.Input;
using Ctor.Models;
using Okna.Plugins.ViewModels;

namespace Ctor.ViewModels
{
    public class ContextViewModel : ViewModelBase
    {
        private readonly FastInsertViewModel _parent;

        internal ContextViewModel(FastInsertViewModel parent)
        {
            _parent = parent;

            this.UseDefaultGlasspacket = true;

            this.SelectWindowTypeCommand = new RelayCommand(SelectWindowType);
            this.SelectWindowColorCommand = new RelayCommand(SelectWindowColor);
            this.SelectGlasspacketCommand = new RelayCommand(SelectGlasspacket);
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

        internal WindowType WindowType { get; private set; }

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

                this.WindowType = _parent.Database.GetWindowType(this.WindowTypeID);
                var colors = this.WindowType.GetProfileColors();
                if (!colors.ContainsColor(this.WindowColorID))
                {
                    this.WindowColorID = 0;
                    this.WindowColorName = null;
                }
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
            if (this.WindowType == null) return;

            var dealer = _parent.Document.Dealer;
            var app = _parent.Application;
            var elementFilter = app.HideAtDealerFilter("koloryp", dealer, "e.");
            elementFilter += " AND e.indeks IN(" + string.Join(",", this.WindowType.GetProfileColors().GetColors()) + ")";

            var dialog = _parent.DialogFactory.GetSearchDialog("SelectTypeColor", elementFilter, null);
            if (this.WindowColorID != 0)
            {
                dialog.Selected = this.WindowColorID;
            }
            if (dialog.ShowDialog(app.MainWindowHWND()) == true)
            {
                this.WindowColorName = (string)dialog.SelectedText;
                this.WindowColorID = (int)dialog.Selected;
            }
        }

        #endregion

        #region Glasspacket

        private int _glasspacketRecno;
        public int GlasspacketRecno
        {
            get { return _glasspacketRecno; }
            set
            {
                if (_glasspacketRecno != value)
                {
                    _glasspacketRecno = value;
                    OnPropertyChanged(nameof(GlasspacketRecno));
                }
            }
        }

        private string _glasspacketNrArt;
        public string GlasspacketNrArt
        {
            get { return _glasspacketNrArt; }
            set
            {
                if (_glasspacketNrArt != value)
                {
                    _glasspacketNrArt = value;
                    OnPropertyChanged(nameof(GlasspacketNrArt));
                }
            }
        }

        private bool _useDefaultGlasspacket;
        public bool UseDefaultGlasspacket
        {
            get { return _useDefaultGlasspacket; }
            set
            {
                if (_useDefaultGlasspacket != value)
                {
                    _useDefaultGlasspacket = value;
                    OnPropertyChanged(nameof(UseDefaultGlasspacket));
                }
            }
        }

        public ICommand SelectGlasspacketCommand { get; private set; }

        private void SelectGlasspacket(object param)
        {
            var dealer = _parent.Document.Dealer;
            var app = _parent.Application;
            var elementFilter = app.HideAtDealerFilter("pak_szyb", dealer, "e.");
            // TODO: filtrování podle dostupných tlouštěk zaklívacích lišt?
            //elementFilter += " AND e.indeks IN(" + string.Join(",", this.WindowType.GetProfileColors().GetColors()) + ")";
            var groupFilter = app.HideAtDealerFilter("gruptech", dealer, "g.");

            var dialog = _parent.DialogFactory.GetSearchDialog("SelectGlazing", elementFilter, groupFilter);
            if (_glasspacketRecno != 0)
            {
                dialog.Selected = _glasspacketRecno;
            }
            if (dialog.ShowDialog(app.MainWindowHWND()) == true)
            {
                this.GlasspacketNrArt = (string)dialog.SelectedText;
                this.GlasspacketRecno = (int)dialog.Selected;
            }
        }

        #endregion
    }
}
