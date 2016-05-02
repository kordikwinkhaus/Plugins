using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Ctor.Models.Scripting;

namespace Ctor.ViewModels
{
    [DebuggerDisplay("{Name,nq} = {Value,nq}")]
    public class VariableViewModel : TreeViewItemViewModel
    {
        private readonly string _name;
        private readonly PropertyInfo _propInfo;
        private readonly Action<object> _setValue;
        private TypeCacheInfo _typeInfo;
        private object _obj;

        internal VariableViewModel(string name, object value)
            : this(name, value, null)
        {
        }

        internal VariableViewModel(string name, object value, VariableViewModel parent)
            : base(parent, true)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            _name = name;
            _setValue = SetValue;
            _setValue(value);
        }

        internal VariableViewModel(PropertyInfo propInfo, object parentObject, VariableViewModel parent)
            : base(parent, true)
        {
            if (propInfo == null) throw new ArgumentNullException(nameof(propInfo));

            _name = propInfo.Name;
            _propInfo = propInfo;
            _setValue = SetPropertyValue;
            _setValue(parentObject);
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
                this.Value = TypeCacheInfo.NULL;
                this.VariableType = "n/a";
            }
        }

        private void SetPropertyValue(object parentValue)
        {
            if (parentValue != null)
            {
                object propValue = null;
                try
                {
                    propValue = _propInfo.GetGetMethod(false).Invoke(parentValue, null);

                    if (propValue != null)
                    {
                        _typeInfo = TypeCache.GetTypeInfo(propValue.GetType());

                        this.VariableType = _typeInfo.Name;
                        this.Value = _typeInfo.GetDebugValue(propValue);

                        if (!_typeInfo.HasPublicProperties)
                        {
                            this.Children.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    string propVal = string.Format("Call threw an exception of type '{0}'.", ex.GetType().ToString());
                    this.Value = propVal;
                    this.VariableType = TypeCache.GetTypeInfo(_propInfo.PropertyType).Name;
                    this.Children.Clear();
                }
            }
            else
            {
                this.Value = TypeCacheInfo.NULL;
                this.VariableType = TypeCache.GetTypeInfo(_propInfo.PropertyType).Name;
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

        internal void Update(object obj)
        {
            _setValue(obj);

            if (_childrenLoaded)
            {
                foreach (var propInfo in _typeInfo.GetPublicProperties())
                {
                    
                    var vm = _children[propInfo.Name];
                    vm.Update(obj);
                }
            }
        }

        private bool _childrenLoaded;
        private Dictionary<string, VariableViewModel> _children;

        protected override void LoadChildren()
        {
            if (_typeInfo != null)
            {
                _children = new Dictionary<string, VariableViewModel>();

                foreach (var propInfo in _typeInfo.GetPublicProperties())
                {
                    var vm = new VariableViewModel(propInfo, _obj, this);
                    _children.Add(propInfo.Name, vm);
                    this.Children.Add(vm);
                }
                _childrenLoaded = true;
            }
        }
    }
}
