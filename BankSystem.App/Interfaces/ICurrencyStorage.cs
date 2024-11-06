using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface ICurrencyStorage
{
    public Task<Guid> GetAsync(string currencyCode, CancellationToken cancellationToken);
    public Task<string> AddAsync(Currency currency, CancellationToken cancellationToken);
    public Task<string> DeleteAsync(string currencyCode, CancellationToken cancellationToken);
}