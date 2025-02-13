namespace FinCs.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}