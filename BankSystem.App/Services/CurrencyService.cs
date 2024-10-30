using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class CurrencyService : ICurrencyService
{
    private ICurrencyStorage _currencyStorage;

    public CurrencyService(ICurrencyStorage currencyStorage)
    {
        _currencyStorage = currencyStorage;
    }
    public async Task<Guid> GetCurrencyAsync(string currencyCode)
    {
        return await _currencyStorage.GetAsync(currencyCode);
    }

    public async Task AddCurrencyAsync(string code, string name, string symbol, decimal exchangeRate)
    {
        try
        {
            var currency = new Currency(code, name, symbol, exchangeRate);
            if (await ValidateCurrencyAsync(currency))
                await _currencyStorage.AddAsync(currency);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task DeleteGurrencyAsync(string currencyCode)
    {
        await _currencyStorage.DeleteAsync(currencyCode);
    }
    private static Task<bool> ValidateCurrencyAsync(Currency currency)
    {
        if (string.IsNullOrWhiteSpace(currency.Code) || string.IsNullOrWhiteSpace(currency.Name) || string.IsNullOrWhiteSpace(currency.Symbol))
        {
            throw new ArgumentException("В метод передана пустая строка (или из пробелов) или null", nameof(currency));
        }

        if (currency.ExchangeRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currency.ExchangeRate), "Стоимость валюты не может быть не положительной.");
        }
        
        return Task.FromResult(true);
    }
}