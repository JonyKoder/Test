using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vodovoz.DAL.Model.Interface;
using Vodovoz.DAL.Repository.Interface;

namespace Vodovoz.DAL.Repository {
    public class Repository<T> : IRepository<T> where T: class, IEntity {
        private readonly VodovozContext _context;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly DbSet<T> _set;
        public Repository(VodovozContext context) => (_context, _set) = (context, context.Set<T>());

        public async Task<bool> AddItemAsync(T item) {
            await _context.AddAsync(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddOrUpdateItemAsync(T item) {
            try {
                if (!IsSupplierRecordValid(item)) return false;
                if (item.ID == 0) return await AddItemAsync(item);
                return await UpdateItemAsync(item);
            } catch (Exception e) {
                _logger.Error(e);
                throw;
            }
        }

        public async Task<bool> DeleteItemAsync(int id) {
            try {
                var item = await _set.SingleAsync(x => x.ID == id);
                _context.Entry(item).State = EntityState.Detached;
                _set.Remove(item);
                return await _context.SaveChangesAsync() > 0;
            } catch (Exception e) {
                _logger.Error(e);
                throw;
            }
        }

        public async Task<T> GetItemAsync(int id) => await _set.SingleOrDefaultAsync(x => x.ID == id);

        public async Task<IEnumerable<T>> GetItemsAsync() => await _set.ToListAsync();

        public async Task<bool> UpdateItemAsync(T item) {
            _context.Entry(item).State = EntityState.Modified;
            _context.Update(item);
            return await _context.SaveChangesAsync() > 0;
        }

        private bool IsSupplierRecordValid(T record) => record.Validate();
    }
}
