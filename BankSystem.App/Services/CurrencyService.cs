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
    public Guid GetGurrency(string currencyCode)
    {
        return _currencyStorage.Get(currencyCode);
    }

    public void AddGurrency(string code, string name, string symbol, decimal exchangeRate)
    {
        try
        {
            var currency = new Currency(code, name, symbol, exchangeRate);
            if (ValidateCurrency(currency))
                _currencyStorage.Add(currency);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public void DeleteGurrency(string currencyCode)
    {
        _currencyStorage.Delete(currencyCode);
    }
    private static bool ValidateCurrency(Currency currency)
    {
        if (string.IsNullOrWhiteSpace(currency.Code) || string.IsNullOrWhiteSpace(currency.Name) || string.IsNullOrWhiteSpace(currency.Symbol))
        {
            throw new ArgumentException("В метод передана пустая строка (или из пробелов) или null", nameof(currency));
        }

        if (currency.ExchangeRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currency.ExchangeRate), "Стоимость валюты не может быть не положительной.");
        }
        
        return true;
    }
}