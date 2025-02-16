using FinCs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess;

internal class FinCsDbContext(DbContextOptions<FinCsDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>().ToTable("expenses");
    }
}