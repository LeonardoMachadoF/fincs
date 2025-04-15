using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess.Repositories;

internal class UserRepository(FinCsDbContext dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    public async Task<bool> ExistsActiveUserWithEmail(string email)
    {
        return await dbContext.Users.AnyAsync(user => user.Email == email);
    }

    public async Task<User?> GetActiveUserWithEmail(string email)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }
}