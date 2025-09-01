using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess.Repositories;

internal class UserRepository(FinCsDbContext dbContext)
    : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    public async Task<bool> ExistsActiveUserWithEmail(string email)
    {
        return await dbContext.Users.AnyAsync(user => user.Email == email);
    }

    public async Task<User?> GetActiveUserWithEmail(string email)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetById(long id)
    {
        return await dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user)
    {
        dbContext.Users.Update(user);
    }

    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }
}