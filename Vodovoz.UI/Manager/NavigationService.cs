using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Vodovoz.DAL.Model;
using Vodovoz.UI.View;

namespace Vodovoz.UI.Manager {
    public delegate void NavigationDelegate(object _sender, NavigationModel _navigateTo);
    public delegate void UpdateValuedNotificationDelegate(object _sender);

    public class NavigationService {
        public List<NavigationModel> Navigations {
            get => NavigationNameToUserControl.Keys.ToList();
        }

        public UserControl NavigateToModel(NavigationModel navigation) {
            if (navigation is null) return null;
            if (NavigationNameToUserControl.ContainsKey(navigation))
                return NavigationNameToUserControl[navigation].Invoke();
            return null;
        }

        public readonly Dictionary<NavigationModel, Func<UserControl>> NavigationNameToUserControl =
            new Dictionary<NavigationModel, Func<UserControl>>
            {
                {new NavigationModel("Сотрудники") ,()=> new EmployeeView()},
                {new NavigationModel("Подразделения"), ()=>new DivisionView()},
                {new NavigationModel("Заказы"),()=> new OrdersView()},
            };

        private static readonly object Instancelock = new object();

        private static NavigationService instance = null;

        public static NavigationService GetInstance {
            get {
                if (instance == null) {
                    lock (Instancelock) {
                        if (instance == null) instance = new NavigationService();
                    }
                }
                return instance;
            }
        }

        public event NavigationDelegate NewNavigationRequested;

        public void RaiseNavigationEven(object _sender, NavigationModel _navigateTo)
            => NewNavigationRequested?.Invoke(_sender, _navigateTo);

        public event UpdateValuedNotificationDelegate ValuesChangedNotification;
    }
}
