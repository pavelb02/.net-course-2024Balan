using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class CurrencyStorage : ICurrencyStorage
{
    private BankSystemDbContext _dbContext;

    public CurrencyStorage()
    {
        _dbContext = new BankSystemDbContext();
    }

    public Guid Get(string currencyCode)
    {
        var currency = _dbContext.Currencies.FirstOrDefault(c => c.Code == currencyCode);
        if (currency==null) throw new ArgumentException($"Валюта с кодом {currencyCode} не найдена.");
        return currency.Id;
    }

    public void Add(Currency currency)
    {
        if (_dbContext.Currencies.Any(c => c.Code == currency.Code))
        {
            throw new InvalidOperationException($"Валюта с кодом {currency.Code} уже существует.");
        }
        _dbContext.Currencies.Add(currency);
        _dbContext.SaveChanges();
    }

    public void Delete(string currencyCode)
    {
        var currency = _dbContext.Currencies.FirstOrDefault(c => c.Code == currencyCode);
        if (currency == null) return;
        _dbContext.Currencies.Remove(currency);
        _dbContext.SaveChanges();
    }
}