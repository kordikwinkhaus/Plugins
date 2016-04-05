using System.ComponentModel;
using System.Diagnostics;

namespace Okna.Plugins.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        [DebuggerStepThrough]
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
