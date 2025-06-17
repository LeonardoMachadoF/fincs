using FinCs.Domain.Entities;

namespace FinCs.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> Get();
}