using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommonLibrary.Wpf
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}