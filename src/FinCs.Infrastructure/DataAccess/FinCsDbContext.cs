using FinCs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess;

public class FinCsDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;Database=fincsdb;Uid=root;Pwd=my-secret-pw;";
        var serverVersion = new MySqlServerVersion(new Version(9, 2, 0));
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>().ToTable("expenses");
    }
}