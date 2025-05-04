using KidsLeisure.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KidsLeisure.DAL.DBContext
{
    public class LeisureDbContextFactory : IDesignTimeDbContextFactory<LeisureDbContext>, ILeisureDbContextFactory
    {
        public LeisureDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LeisureDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LeisureDb;Trusted_Connection=True;");

            var context = new LeisureDbContext(optionsBuilder.Options);
            return context;
        }
        public Task<ILeisureDbContext> CreateDbContextAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LeisureDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LeisureDb;Trusted_Connection=True;");

            var context = new LeisureDbContext(optionsBuilder.Options);
            return Task.FromResult(context as ILeisureDbContext);
        }
    }
}
