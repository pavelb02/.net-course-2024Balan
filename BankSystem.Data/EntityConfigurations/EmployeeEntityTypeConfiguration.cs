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
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Surname).HasMaxLength(100).IsRequired();
        builder.Property(p => p.NumPassport).IsRequired();
        builder.Property(p => p.Phone).IsRequired();
        builder.Property(p => p.DateBirthday).IsRequired();

        builder.Property(c => c.Position);
        builder.Property(c => c.Salary);
        builder.Property(c => c.StartDate);
        builder.Property(c => c.Contract);

        builder.HasKey(p => p.Id);
    }
}