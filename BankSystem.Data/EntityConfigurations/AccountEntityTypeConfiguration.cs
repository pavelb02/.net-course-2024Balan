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
        builder.Property(a => a.Amount);

        builder.HasKey(a => a.Id);
        builder.HasOne(c => c.Client).WithMany(co => co.AccountsClient).HasForeignKey(a => a.ClientId);
        builder.HasOne(c => c.Currency).WithMany(co => co.AccountsCurrency).HasForeignKey(a => a.CurrencyId);
    }
}