using Microsoft.EntityFrameworkCore;

namespace KidsLeisure.Core.Interfaces
{
    public interface ILeisureDbContextFactory
    {
        Task<ILeisureDbContext> CreateDbContextAsync();
    }
}
