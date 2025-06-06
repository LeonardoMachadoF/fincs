using FinCs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess;

public class FinCsDbContext(DbContextOptions<FinCsDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>().ToTable("expenses");
        modelBuilder.Entity<User>().ToTable("users");
    }
}