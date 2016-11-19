using System.Windows.Input;
using Okna.Plugins;
using Okna.Plugins.ViewModels;

namespace WindowOffset.ViewModels
{
    public class OffsetItemViewModel : ViewModelBase
    {
        private int _parentOffset;

        public OffsetItemViewModel()
        {
            this.ResetValueCommand = new RelayCommand(ResetValue, CanResetValue);
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public ICommand ResetValueCommand { get; private set; }

        private bool CanResetValue(object param)
        {
            return this.HasOwnValue;
        }

        private void ResetValue(object param)
        {
            if (this.CanResetValue(param))
            {
                _offset = _parentOffset;
                _hasOwnValue = false;

                OnPropertyChanged(nameof(Offset));
                OnPropertyChanged(nameof(HasOwnValue));
            }
        }

        private int _offset;
        public virtual int Offset
        {
            get { return _offset; }
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    OnPropertyChanged(nameof(Offset));
                    HasOwnValue = true;
                }
            }
        }

        private bool _hasOwnValue;
        public bool HasOwnValue
        {
            get { return _hasOwnValue; }
            set
            {
                if (_hasOwnValue != value)
                {
                    _hasOwnValue = value;
                    OnPropertyChanged(nameof(HasOwnValue));
                }
            }
        }

        internal void TrySetParentValue(int value)
        {
            _parentOffset = value;
            if (!this.HasOwnValue)
            {
                _offset = value;
                OnPropertyChanged(nameof(Offset));
            }
        }
    }
}
