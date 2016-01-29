using System.ComponentModel;

namespace Okna.Plugins.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
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
