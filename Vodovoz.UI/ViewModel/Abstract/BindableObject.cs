using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Vodovoz.UI.ViewModel.Abstract {
    public abstract class BindableObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void UpdateProperties(List<string> propertyNames) {
            foreach (var propertyName in propertyNames) 
                OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string name) {
            var propertyChanged = PropertyChanged;
            if (propertyChanged is not null) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string name = null) {
            if (EqualityComparer<T>.Default.Equals(item,value)) {
                item = value;
                OnPropertyChanged(name);
            }
        }
    }
}
