using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.Property(p => p.Id);
        builder.Property(p => p.Name);
        builder.Property(p => p.Surname);
        builder.Property(p => p.NumPassport);
        builder.Property(p => p.Phone);
        builder.Property(p => p.DateBirthday);

        builder.Property(c => c.Position);
        builder.Property(c => c.Salary);
        builder.Property(c => c.StartDate);
        builder.Property(c => c.Contract);

        builder.HasKey(p => p.Id);
    }
}