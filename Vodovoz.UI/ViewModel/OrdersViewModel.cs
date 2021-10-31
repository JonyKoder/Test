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
    public class OrdersViewModel : BindableObject {
        private readonly VodovozContext _context;
        private readonly IRepository<Orders> _repositoryOrders;
        private readonly IRepository<Employee> _repositoryEmployees;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ObservableCollection<OrdersWrapper> Orders { get; set; } = new();
        public ObservableCollection<Employee> Employees { get; set; } = new();

        private async void RefreshOrdersAndEmployees() {
            Orders.Clear();
            Employees.Clear();
            var orders = await _repositoryOrders.GetItemsAsync();
            orders.ToList().ForEach(x => {
                var temp = new OrdersWrapper(x);
                temp.ItemSelected += OrdersItemSelected;
                Orders.Add(temp);
            });
            var employees = await _repositoryEmployees.GetItemsAsync();
            employees.ToList().ForEach(x => Employees.Add(x));
            OnPropertyChanged(nameof(Orders));
            OnPropertyChanged(nameof(Employees));
        }

        public OrdersViewModel() {
            _context = new();
            _repositoryOrders = new Repository<Orders>(_context);
            _repositoryEmployees = new Repository<Employee>(_context);
            RefreshOrdersAndEmployees();
        }

        private OrdersWrapper _editerOrders = new(new Orders());

        public OrdersWrapper EditerOrders {
            get => _editerOrders;
            set {
                _editerOrders = value;
                OnPropertyChanged(nameof(EditerOrders));
            }
        }

        public ICommand CancelCommand { get => new ActionCommand((obj) => ResetEditableOrders(obj)); }
        public ICommand ModifySelectedEmployeeCommand { get => new ActionCommand(async (obj) => await ModifySelectedOrders()); }

        private void ResetEditableOrders(object para) {
            _editerOrders = new(new Orders());
            OnPropertyChanged(nameof(EditerOrders));
        }
        private async Task ModifySelectedOrders() {
            try {
                if (!await _repositoryOrders.AddOrUpdateItemAsync(EditerOrders.GetOrders))
                    throw new Exception("Something went wrong during the Process. Please try again later...");
                EditerOrders = new(new Orders());
                RefreshOrdersAndEmployees();
            } catch (Exception e) {
                _logger.Error(e);
                MessageBox.Show($"{e.Message}");
            }
        }
        private async void OrdersItemSelected(object sender, object sendObject) {
            if (sendObject.ToString() == "Remove") {
                if (!await _repositoryOrders.DeleteItemAsync((sender as OrdersWrapper).GetOrders.ID))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
                RefreshOrdersAndEmployees();
            } else EditerOrders = (OrdersWrapper)sender;
        }
    }
}
