using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.Property(a => a.Id);
        builder.Property(a => a.Currency);
        builder.Property(a => a.Amount);

        builder.HasKey(a => a.Id);
        builder.HasOne(c => c.Client).WithMany(co => co.Accounts).HasForeignKey(a => a.ClientId);
    }
}