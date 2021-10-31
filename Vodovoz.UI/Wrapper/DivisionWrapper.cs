using System.Windows.Input;
using Vodovoz.DAL.Model;
using Vodovoz.UI.ViewModel;
using Vodovoz.UI.ViewModel.Command;

namespace Vodovoz.UI.Wrapper {
    public class DivisionWrapper : Division {
        private readonly Division _division;
        public event SelectedItemDelegate ItemSelected;

        public DivisionWrapper(Division division) => _division = division;

        public Division GetDivision {
            get => _division;
        }
        public ICommand Selected {
            get => new ActionCommand((obj) => {
                ItemSelected?.Invoke(this, obj);
            });
        }
    }
}
