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
        builder.Property(p => p.Name);
        builder.Property(p => p.Surname);
        builder.Property(p => p.NumPassport);
        builder.Property(p => p.Phone);
        builder.Property(p => p.DateBirthday);

        builder.Property(c => c.Balance);
        builder.Property(c => c.AccountNumber);

        builder.HasKey(p => p.Id);
        builder.HasMany(c => c.Accounts).WithOne(co => co.Client).HasForeignKey(a => a.ClientId);
    }
}