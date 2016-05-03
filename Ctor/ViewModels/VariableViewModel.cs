using System;
using System.Collections;
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
        private bool _wasNull;

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
                if (_wasNull)
                {
                    this.AddDummyChild();
                    _wasNull = false;
                }

                var oldTypeInfo = _typeInfo;
                _typeInfo = TypeCache.GetTypeInfo(value.GetType());
                if (_typeInfo != oldTypeInfo)
                {
                    this.Children.Clear();
                    if (_typeInfo.HasPublicProperties)
                    {
                        this.IsExpanded = false;
                        this.AddDummyChild();
                    }
                }

                this.VariableType = _typeInfo.Name;
                this.Value = _typeInfo.GetDebugValue(value);
            }
            else
            {
                _wasNull = true;
                this.Value = TypeCacheInfo.NULL;
                this.VariableType = "n/a";
                this.Children.Clear();
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
                        _obj = propValue;
                        _typeInfo = TypeCache.GetTypeInfo(propValue.GetType());

                        this.VariableType = _typeInfo.Name;
                        this.Value = _typeInfo.GetDebugValue(propValue);

                        if (!_typeInfo.HasPublicProperties)
                        {
                            this.Children.Clear();
                        }
                    }
                    else
                    {
                        this.Value = TypeCacheInfo.NULL;
                        this.VariableType = TypeCache.GetTypeInfo(_propInfo.PropertyType).Name;
                        this.Children.Clear();
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
                if (_childrenManager.IsSuitableFor(obj))
                {
                    _childrenManager.Update(obj);
                }
                else
                {
                    this.Children.Clear();
                    _childrenManager = null;
                    this.LoadChildren();
                }
            }
        }

        private bool _childrenLoaded;
        private ChildrenManager _childrenManager;

        protected override void LoadChildren()
        {
            if (_obj != null)
            {
                var type = _obj.GetType();
                if (DictionaryChildrenManager.IsSuitableFor(type))
                {
                    _childrenManager = new DictionaryChildrenManager(this);
                }
                else if (ListChildrenManager.IsSuitableFor(type))
                {
                    _childrenManager = new ListChildrenManager(this);
                }
            }

            if (_childrenManager == null)
            {
                _childrenManager = new PublicPropertiesChildrenManager(this);
            }
            _childrenManager.Init(_obj);
            _childrenLoaded = true;
        }

        #region nested classes

        internal abstract class ChildrenManager
        {
            protected readonly VariableViewModel _parent;

            internal ChildrenManager(VariableViewModel parent)
            {
                _parent = parent;
            }

            internal abstract bool IsSuitableFor(object obj);

            internal abstract void Init(object obj);

            internal abstract void Update(object obj);
        }

        internal class PublicPropertiesChildrenManager : ChildrenManager
        {
            private Dictionary<string, VariableViewModel> _children;
            private TypeCacheInfo _typeInfo;

            internal PublicPropertiesChildrenManager(VariableViewModel parent) 
                : base(parent)
            {
                _children = new Dictionary<string, VariableViewModel>();
            }

            internal override bool IsSuitableFor(object obj)
            {
                if (obj == null) return true;

                var type = obj.GetType();
                return (TypeCache.GetTypeInfo(type) == _typeInfo);
            }

            internal override void Init(object obj)
            {
                _typeInfo = _parent._typeInfo;
                foreach (var propInfo in _typeInfo.GetPublicProperties())
                {
                    var vm = new VariableViewModel(propInfo, obj, _parent);
                    _children.Add(propInfo.Name, vm);
                    _parent.Children.Add(vm);
                }
            }

            internal override void Update(object obj)
            {
                foreach (var propInfo in _typeInfo.GetPublicProperties())
                {
                    var vm = _children[propInfo.Name];
                    vm.Update(obj);
                }
            }
        }

        internal class ListChildrenManager : ChildrenManager
        {
            internal static bool IsSuitableFor(Type type)
            {
                return TypeHelper.ImplementsInterface(type, typeof(IList)) ||
                       TypeHelper.ImplementsInterface(type, typeof(IList<>));
            }

            internal ListChildrenManager(VariableViewModel parent) 
                : base(parent)
            {
            }

            internal override bool IsSuitableFor(object obj)
            {
                if (obj == null) return true;
                var type = obj.GetType();

                return IsSuitableFor(type);
            }

            internal override void Init(object obj)
            {
                var list = (IList)obj;
                for (int i = 0; i < list.Count; i++)
                {
                    var vm = new VariableViewModel("[" + i + "]", list[i], _parent);
                    _parent.Children.Add(vm);
                }
            }

            internal override void Update(object obj)
            {
                var list = (IList)obj;
                if (list != null)
                {
                    int toUpdateCount = Math.Min(_parent.Children.Count, list.Count);
                    for (int i = 0; i < toUpdateCount; i++)
                    {
                        ((VariableViewModel)(_parent.Children[i])).Update(list[i]);
                    }

                    if (list.Count > _parent.Children.Count)
                    {
                        for (int i = toUpdateCount; i < list.Count; i++)
                        {
                            var vm = new VariableViewModel("[" + i + "]", list[i], _parent);
                            _parent.Children.Add(vm);
                        }
                    }
                    else if (_parent.Children.Count > list.Count)
                    {
                        for (int i = _parent.Children.Count - 1; i >= toUpdateCount; i--)
                        {
                            _parent.Children.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    _parent.Children.Clear();
                }
            }
        }

        internal class DictionaryChildrenManager : ChildrenManager
        {
            internal static bool IsSuitableFor(Type type)
            {
                return TypeHelper.ImplementsInterface(type, typeof(IDictionary));
            }

            private Dictionary<string, VariableViewModel> _children;

            internal DictionaryChildrenManager(VariableViewModel parent) 
                : base(parent)
            {
            }

            internal override bool IsSuitableFor(object obj)
            {
                if (obj == null) return true;
                var type = obj.GetType();

                return TypeHelper.ImplementsInterface(type, typeof(IDictionary));
            }

            internal override void Init(object obj)
            {
                _children = new Dictionary<string, VariableViewModel>();
                var dict = (IDictionary)obj;
                foreach (object key in dict.Keys)
                {
                    string name = TypeCacheInfo.NULL;
                    if (key != null)
                    {
                        var typeInfo = TypeCache.GetTypeInfo(key.GetType());
                        name = typeInfo.GetDebugValue(key);
                    }
                    name = "[" + name + "]";

                    var vm = new VariableViewModel(name, dict[key], _parent);
                    _parent.Children.Add(vm);
                    _children.Add(name, vm);
                }
            }

            internal override void Update(object obj)
            {
                var newChildren = new Dictionary<string, VariableViewModel>();

                var dict = (IDictionary)obj;
                if (dict != null)
                {
                    foreach (object key in dict.Keys)
                    {
                        string name = TypeCacheInfo.NULL;
                        if (key != null)
                        {
                            var typeInfo = TypeCache.GetTypeInfo(key.GetType());
                            name = typeInfo.GetDebugValue(key);
                        }
                        name = "[" + name + "]";

                        VariableViewModel vm;
                        if (_children.TryGetValue(name, out vm))
                        {
                            vm.Update(dict[key]);
                            _children.Remove(name);
                        }
                        else
                        {
                            vm = new VariableViewModel(name, dict[key], _parent);
                            _parent.Children.Add(vm);
                        }
                        newChildren.Add(name, vm);
                    }

                    foreach (var value in _children.Values)
                    {
                        _parent.Children.Remove(value);
                    }

                    _children = newChildren;
                }
                else
                {
                    _parent.Children.Clear();
                }
            }
        }

        #endregion
    }
}
