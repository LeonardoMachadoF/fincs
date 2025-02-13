namespace FinCs.Domain.Repositories;

public interface IUnitOfWork
{
    void Commit();
}