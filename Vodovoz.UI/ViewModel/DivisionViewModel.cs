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
    public class DivisionViewModel : BindableObject {
        private readonly VodovozContext _context;
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository<Division> _repositoryDivision;

        public ObservableCollection<Employee> Employees { get; set; } = new();
        public ObservableCollection<DivisionWrapper> Division { get; set; } = new();

        private async void RePopulateEmployees() {
            Employees.Clear();
            var employees = await _repositoryEmployee.GetItemsAsync();
            employees.ToList().ForEach(x => Employees.Add(x));
            OnPropertyChanged(nameof(Employees));
            await RefreshDivisionsAsync();
        }
        private async Task RefreshDivisionsAsync() {
            Division.Clear();
            var divisions = await _repositoryDivision.GetItemsAsync();
            divisions.ToList().ForEach(x => {
                var temp = new DivisionWrapper(x);
                temp.ItemSelected += DivisionItemSelected;
                Division.Add(temp);
            });
            OnPropertyChanged(nameof(Division));
        }

        public DivisionViewModel() {
            _context = new ();
            _repositoryEmployee = new Repository<Employee>(_context);
            _repositoryDivision = new Repository<Division>(_context);
            RePopulateEmployees();
        }

        private DivisionWrapper _editerDivision = new(new Division());

        public DivisionWrapper EditerDivision {
            get => _editerDivision;
            set {
                _editerDivision = value;
                OnPropertyChanged(nameof(EditerDivision));
            }
        }

        public ICommand ModifySelectedDivisionCommand { get => new ActionCommand(async (obj) => await ModifySelectedDivision()); }
        public ICommand CancelCommand { get => new ActionCommand((obj) => ResetEditableDivision(obj)); }

        private async Task ModifySelectedDivision() {
            try {
                if (!await _repositoryDivision.AddOrUpdateItemAsync(EditerDivision.GetDivision))
                    throw new Exception("Something went wrong during the Process. Please try again later...");
                EditerDivision = new(new Division());
                RePopulateEmployees();
            } catch (Exception e) {
                _logger.Error(e);
                MessageBox.Show($"{e.Message}");
            }
        }

        private void ResetEditableDivision(object para) {
            _editerDivision = new(new Division());
            OnPropertyChanged(nameof(EditerDivision));
        }

        private async void DivisionItemSelected(object sender, object sendObject) {
            if (sendObject.ToString() == "Remove") {
                if (!await _repositoryEmployee.DeleteItemAsync((sender as DivisionWrapper).GetDivision.ID))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
                RePopulateEmployees();
            } else EditerDivision = (DivisionWrapper)sender;
        }
    }
}
