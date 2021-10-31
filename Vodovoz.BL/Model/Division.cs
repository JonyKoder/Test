using System.Collections.Generic;
using Vodovoz.DAL.Model.Interface;

namespace Vodovoz.DAL.Model {
    public class Division: IEntity {
        public int ID { get; set; }
        public string Title { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public Employee Boss { get; set; }

        public int BossId { get; set; }
        public bool Validate() {
            if (string.IsNullOrWhiteSpace(Title)) return false;
            return true;
        }
    }
}