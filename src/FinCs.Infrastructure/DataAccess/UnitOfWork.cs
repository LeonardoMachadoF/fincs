using FinCs.Domain.Repositories;

namespace FinCs.Infrastructure.DataAccess;

internal class UnitOfWork(FinCsDbContext dbContext) : IUnitOfWork
{
    public void Commit()
    {
        dbContext.SaveChanges();
    }
}