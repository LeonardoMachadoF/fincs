using FinCs.Domain.Repositories;

namespace FinCs.Infrastructure.DataAccess;

internal class UnitOfWork(FinCsDbContext dbContext) : IUnitOfWork
{
    public async Task Commit()
    {
        await dbContext.SaveChangesAsync();
    }
}