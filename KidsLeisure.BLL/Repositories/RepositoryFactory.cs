using KidsLeisure.DAL.DBContext;
using KidsLeisure.BLL.Interfaces;

namespace KidsLeisure.BLL.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly LeisureDbContext _context;

        public RepositoryFactory(LeisureDbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }
    }
}