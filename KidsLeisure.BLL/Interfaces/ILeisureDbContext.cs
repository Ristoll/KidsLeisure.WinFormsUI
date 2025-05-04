using Microsoft.EntityFrameworkCore;

namespace KidsLeisure.Core.Interfaces
{
    public interface ILeisureDbContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
