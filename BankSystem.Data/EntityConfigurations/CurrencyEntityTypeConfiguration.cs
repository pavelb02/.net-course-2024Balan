using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class CurrencyEntityTypeConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currencies");
        builder.Property(c => c.Code).IsRequired();
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Symbol);
        builder.Property(c => c.ExchangeRate).IsRequired();
        
        builder.HasKey(a => a.Id);
        builder.HasMany(c=>c.AccountsCurrency).WithOne(co=>co.Currency).HasForeignKey(a=>a.CurrencyId);

    }
}