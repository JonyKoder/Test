using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NLog;
using Vodovoz.DAL;
using Vodovoz.DAL.Model;
using Vodovoz.DAL.Repository;
using Vodovoz.DAL.Repository.Interface;
using Vodovoz.UI.ViewModel.Abstract;
using Vodovoz.UI.ViewModel.Command;
using Vodovoz.UI.Wrapper;

namespace Vodovoz.UI.ViewModel {
    public delegate void SelectedItemDelegate(object sender, object sendObject);

    public class EmployeeViewModel : BindableObject {
        private readonly VodovozContext _context;
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository<Division> _repositoryDivision;
        public ObservableCollection<EmployeeWrapper> Employees { get; set; } = new();
        public ObservableCollection<Division> Division { get; set; } = new();
        public ObservableCollection<Gender> Genders { get; set; } = new();

        private async void RefreshEmployees() {
            Employees.Clear();
            var employees = await _repositoryEmployee.GetItemsAsync();
            employees.ToList().ForEach(x => {
                var temp = new EmployeeWrapper(x);
                temp.ItemSelected += SupplierItemSelected;
                Employees.Add(temp);
            });
            OnPropertyChanged(nameof(Employees));
            await RefreshDivisions();
        }
        private async Task RefreshDivisions() {
            Division.Clear();
            var divisions = await _repositoryDivision.GetItemsAsync();
            divisions.ToList().ForEach(x => Division.Add(x));
            OnPropertyChanged(nameof(Division));
        }

        public EmployeeViewModel() {
            _context = new ();
            _repositoryEmployee = new Repository<Employee>(_context);
            _repositoryDivision = new Repository<Division>(_context);
            RefreshEmployees();
            Genders.Add(Gender.Male);
            Genders.Add(Gender.Female);
        }

        private EmployeeWrapper _editerEmployee = new(new Employee());

        public EmployeeWrapper EditerEmployee {
            get => _editerEmployee;
            set {
                _editerEmployee = value;
                OnPropertyChanged(nameof(EditerEmployee));
            }
        }

        public ICommand CancelCommand { get => new ActionCommand((obj) => ResetEditableEmployee(obj)); }
        public ICommand ModifySelectedEmployeeCommand {
            get => new ActionCommand(async (obj) => await ModifySelectedEmployee());
        }

        private async Task ModifySelectedEmployee() {
            try {
                if (!await _repositoryEmployee.AddOrUpdateItemAsync(EditerEmployee.GetEmployee))
                    throw new Exception("Something went wrong during the Process. Please try again later...");
                EditerEmployee = new(new Employee());
                RefreshEmployees();
            } catch (Exception e) {
                _logger.Error(e);
                MessageBox.Show($"{e.Message}");
            }
        }
        private void ResetEditableEmployee(object para) {
            _editerEmployee = new(new Employee());
            OnPropertyChanged(nameof(EditerEmployee));
        }
        private async void SupplierItemSelected(object sender, object sendObject) {
            if (sendObject.ToString() == "Remove") {
                if (!await _repositoryEmployee.DeleteItemAsync((sender as EmployeeWrapper).GetEmployee.ID))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
                RefreshEmployees();
            } else EditerEmployee = (EmployeeWrapper)sender;
        }
    }
}
