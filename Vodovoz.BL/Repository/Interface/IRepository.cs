using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vodovoz.DAL.Repository.Interface {
    public interface IRepository<T> {
        Task<IEnumerable<T>> GetItemsAsync();

        Task<T> GetItemAsync(int id);

        Task<bool> AddOrUpdateItemAsync(T item);

        Task<bool> AddItemAsync(T item);

        Task<bool> UpdateItemAsync(T item);

        Task<bool> DeleteItemAsync(int id);
    }
}
