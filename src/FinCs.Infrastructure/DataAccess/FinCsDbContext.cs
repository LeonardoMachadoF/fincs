using FinCs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess;

internal class FinCsDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>().ToTable("expenses");
    }
}