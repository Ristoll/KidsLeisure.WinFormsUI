using KidsLeisure.DAL.Entities;
using System.Linq.Expressions;

namespace KidsLeisure.BLL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<OrderEntity> GetByIdWithIncludesAsync(int orderId, bool includeZones, bool includeAttractions, bool includeCharacters);
    }
}