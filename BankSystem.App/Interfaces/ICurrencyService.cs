using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface ICurrencyService
{
    public Task<Guid> GetGurrencyAsync(string currencyCode);
    public Task AddGurrencyAsync(string code, string name, string symbol, decimal exchangeRate);
    public Task DeleteGurrencyAsync(string currencyCode);
}