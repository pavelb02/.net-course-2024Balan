using System.Net.Mime;
using System.Security.AccessControl;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data;

public class BankSystemDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Account> Accounts { get; set; }

    public BankSystemDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost; Port=5432; Database=BankSystem; Username=postgres; Password=postgres");
    }

    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null) throw new ArgumentException(nameof(modelBuilder));
        modelBuilder.ApplyConfiguration(new ClientEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}