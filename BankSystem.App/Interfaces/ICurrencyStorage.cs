using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface ICurrencyStorage
{
    public Task<Guid> GetAsync(string currencyCode);
    public Task AddAsync(Currency currency);
    public Task DeleteAsync(string currencyCode);
}