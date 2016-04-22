using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.ViewModels;

namespace Ctor.ViewModels
{
    [DebuggerDisplay("{Name,nq} = {Value,nq}")]
    public class VariableViewModel : ViewModelBase
    {
        private readonly string _name;

        internal VariableViewModel(string name, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            _name = name;
            SetValue(value);
        }

        public string Name
        {
            get { return _name; }
        }

        private void SetValue(object value)
        {
            if (value != null)
            {
                var convertible = value as IConvertible;
                if (convertible != null)
                {
                    this.Value = convertible.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    this.Value = value.ToString();
                }
                this.VariableType = value.GetType().ToString();
            }
            else
            {
                this.Value = "None (null)";
                this.VariableType = "n/a";
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            private set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        private string _variableType;
        public string VariableType
        {
            get { return _variableType; }
            private set
            {
                if (_variableType != value)
                {
                    _variableType = value;
                    OnPropertyChanged(nameof(VariableType));
                }
            }
        }

        internal void Update(object value)
        {
            SetValue(value);
        }
    }
}
