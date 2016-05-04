using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.ViewModels;

namespace Ctor.ViewModels
{
    [DebuggerDisplay("Count = {Count}")]
    public class VariablesViewModel : ViewModelBase
    {
        private readonly Dictionary<string, VariableViewModel> _varsDict;

        public VariablesViewModel(IDictionary<object, object> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            _varsDict = new Dictionary<string, VariableViewModel>();
            this.Variables = new ObservableCollection<VariableViewModel>();
            foreach (var kvp in dictionary)
            {
                string name = kvp.Key.ToString();
                var vm = new VariableViewModel(name, kvp.Value);
                this.Variables.Add(vm);
                _varsDict.Add(name, vm);
            }
        }

        public int Count
        {
            get { return this.Variables.Count; }
        }

        public ObservableCollection<VariableViewModel> Variables { get; private set; }

        internal void Clear()
        {
            _varsDict.Clear();
            this.Variables.Clear();
        }

        internal void Update(IDictionary<object, object> dictionary)
        {
            List<string> variableNames = _varsDict.Keys.ToList();

            foreach (var kvp in dictionary)
            {
                VariableViewModel vm;
                string name = kvp.Key.ToString();
                if (_varsDict.TryGetValue(name, out vm))
                {
                    variableNames.Remove(name);
                    vm.Update(kvp.Value);
                }
                else
                {
                    vm = new VariableViewModel(name, kvp.Value);
                    this.Variables.Add(vm);
                    _varsDict.Add(name, vm);
                }
            }

            foreach (var name in variableNames)
            {
                var vm = _varsDict[name];
                _varsDict.Remove(name);
                this.Variables.Remove(vm);
            }
        }

        internal void UpdateSpecial(string specialName, object payload)
        {
            var vm = new VariableViewModel(specialName, payload);
            this.Variables.Add(vm);
            _varsDict.Add(specialName, vm);
        }
    }
}
