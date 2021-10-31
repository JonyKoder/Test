using System.Windows.Input;
using Vodovoz.DAL.Model;
using Vodovoz.UI.ViewModel;
using Vodovoz.UI.ViewModel.Command;

namespace Vodovoz.UI.Wrapper {
    public class EmployeeWrapper : Employee {
        private readonly Employee _employee;
        public event SelectedItemDelegate ItemSelected;

        public EmployeeWrapper(Employee employee) => _employee = employee;

        public Employee GetEmployee {
            get => _employee;
        }

        public ICommand Selected {
            get => new ActionCommand((obj) => {
                ItemSelected?.Invoke(this, obj);
            });
        }
    }
}
