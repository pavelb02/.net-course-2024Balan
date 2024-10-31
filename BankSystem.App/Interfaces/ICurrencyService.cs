using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface ICurrencyService
{
    public Task<Guid> GetCurrencyAsync(string currencyCode);
    public Task AddCurrencyAsync(string code, string name, string symbol, decimal exchangeRate);
    public Task DeleteGurrencyAsync(string currencyCode);
}