using KidsLeisure.BLL.Interfaces;
using KidsLeisure.BLL.Repositories;
using KidsLeisure.DAL.DBContext;

namespace KidsLeisure.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LeisureDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private bool _disposed = false;

        public UnitOfWork(LeisureDbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repo))
                return (IRepository<T>)repo;

            var newRepo = new Repository<T>(_context);
            _repositories[typeof(T)] = newRepo;
            return newRepo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
