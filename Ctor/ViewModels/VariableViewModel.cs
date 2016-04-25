using System;
using System.Diagnostics;
using Ctor.Models.Scripting;

namespace Ctor.ViewModels
{
    [DebuggerDisplay("{Name,nq} = {Value,nq}")]
    public class VariableViewModel : TreeViewItemViewModel
    {
        private readonly string _name;
        private TypeCacheInfo _typeInfo;
        private object _obj;

        internal VariableViewModel(string name, object value)
            : this(name, value, null)
        {
        }

        public VariableViewModel(string name, object value, VariableViewModel parent) 
            : base(parent, true)
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
            _obj = value;
            if (value != null)
            {
                _typeInfo = TypeCache.GetTypeInfo(value.GetType());

                this.VariableType = _typeInfo.Name;
                this.Value = _typeInfo.GetDebugValue(value);

                if (!_typeInfo.HasPublicProperties)
                {
                    this.Children.Clear();
                }
            }
            else
            {
                this.Value = "None";
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

        protected override void LoadChildren()
        {
            if (_typeInfo != null)
            {
                foreach (var propInfo in _typeInfo.GetPublicProperties())
                {
                    object value = null;
                    if (_obj != null)
                    {
                        value = propInfo.GetGetMethod(false).Invoke(_obj, null);
                    }
                    var vm = new VariableViewModel(propInfo.Name, value, this);
                    this.Children.Add(vm);
                }
            }
        }
    }
}
