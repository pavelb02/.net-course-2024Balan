using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Storages;

public class CurrencyStorage : ICurrencyStorage
{
    private readonly BankSystemDbContext _dbContext;

    public CurrencyStorage()
    {
        _dbContext = new BankSystemDbContext();
    }

    public async Task<Guid> GetAsync(string currencyCode)
    {
        var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);
        if (currency==null) throw new ArgumentException($"Валюта с кодом {currencyCode} не найдена.");
        return currency.Id;
    }

    public async Task AddAsync(Currency currency)
    {
        if (await _dbContext.Currencies.AnyAsync(c => c.Code == currency.Code))
        {
            throw new InvalidOperationException($"Валюта с кодом {currency.Code} уже существует.");
        }
        _dbContext.Currencies.Add(currency);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(string currencyCode)
    {
        var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);
        if (currency == null) return;
        _dbContext.Currencies.Remove(currency);
        await _dbContext.SaveChangesAsync();
    }
}