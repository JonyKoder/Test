using Vodovoz.DAL.Model;
using Vodovoz.UI.Manager;
using Vodovoz.UI.ViewModel.Abstract;

namespace Vodovoz.UI.ViewModel {
    public class MainViewModel: BindableObject {
        public NavigationModel CurrentView { get; set; }

        public MainViewModel() => NavigationService.GetInstance.NewNavigationRequested += GetInstance_NewNavigationRequested;

        private void GetInstance_NewNavigationRequested(object _sender, NavigationModel _navigateTo) {
            CurrentView = _navigateTo;
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
