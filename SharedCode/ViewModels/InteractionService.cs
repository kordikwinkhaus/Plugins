using System;
using System.Collections.Generic;
using System.Windows;

namespace Okna.Plugins.ViewModels
{
    public class InteractionService : IInteractionService
    {
        private readonly Dictionary<Type, Type> _dialogs;
        private readonly string _title;

        public InteractionService(string title)
        {
            _dialogs = new Dictionary<Type, Type>();
            _title = title;
        }

        public void Register<TView, TViewModel>()
        {
            _dialogs.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog<T>(T viewModel)
        {
            Type viewType;
            if (!_dialogs.TryGetValue(typeof(T), out viewType))
            {
                throw new InvalidOperationException(string.Format("There is no registered view for viewmodel of type '{0}'.", typeof(T)));
            }
            var view = (Window)(Activator.CreateInstance(viewType));
            view.DataContext = viewModel;
            return view.ShowDialog();
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
