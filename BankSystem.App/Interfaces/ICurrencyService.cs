using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface ICurrencyService
{
    public Guid GetGurrency(string currencyCode);
    public void AddGurrency(string code, string name, string symbol, decimal exchangeRate);
    public void DeleteGurrency(string currencyCode);
}