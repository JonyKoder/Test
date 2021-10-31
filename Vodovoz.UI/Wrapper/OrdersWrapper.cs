using System.Windows.Input;
using Vodovoz.DAL.Model;
using Vodovoz.UI.ViewModel;
using Vodovoz.UI.ViewModel.Command;

namespace Vodovoz.UI.Wrapper {
    public class OrdersWrapper : Orders {
        private readonly Orders _orders;
        public event SelectedItemDelegate ItemSelected;

        public OrdersWrapper(Orders orders) => _orders = orders;

        public Orders GetOrders {
            get => _orders;
        }

        public ICommand Selected {
            get => new ActionCommand((obj) => {
                ItemSelected?.Invoke(this, obj);
            });
        }
    }
}
