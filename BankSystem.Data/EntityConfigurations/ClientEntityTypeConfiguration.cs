using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.Property(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Surname).HasMaxLength(100).IsRequired();
        builder.Property(p => p.NumPassport).IsRequired();
        builder.Property(p => p.Phone).IsRequired();
        builder.Property(p => p.DateBirthday).HasColumnType("date").IsRequired();

       builder.HasKey(p => p.Id);
        builder.HasMany(c => c.AccountsClient).WithOne(co => co.Client).HasForeignKey(a => a.ClientId);
    }
}