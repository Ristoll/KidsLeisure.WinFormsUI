using KidsLeisure.DAL.DBContext;
using KidsLeisure.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using KidsLeisure.DAL.Entities;

namespace KidsLeisure.BLL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LeisureDbContext _context;

        public Repository(LeisureDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id)
                         ?? throw new KeyNotFoundException($"Entity with ID {id} not found.");
            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<OrderEntity> GetByIdWithIncludesAsync(int orderId, bool includeZones, bool includeAttractions, bool includeCharacters)
        {
            IQueryable<OrderEntity> query = _context.Orders;

            if (includeZones)
                query = query.Include(o => o.Zones).ThenInclude(oz => oz.Zone);
            if (includeAttractions)
                query = query.Include(o => o.Attractions).ThenInclude(oa => oa.Attraction);
            if (includeCharacters)
                query = query.Include(o => o.Characters).ThenInclude(oc => oc.Character);

            return await query.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }



    }
}
