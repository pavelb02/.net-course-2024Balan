using System.Reflection.Metadata.Ecma335;

namespace BankSystem.Domain.Models;

public struct Currency
{
    public string Code { get; set; } // Код валюты (USD, EUR, RUB)
    public string Name { get; set; } // Название валюты (доллар США, евро, российский рубль)
    public string Symbol { get; set; } // Символ валюты ($, €, ₽)
    public decimal ExchangeRate { get; set; } // Курс валюты

    public Currency(string code, string name, string symbol, decimal exchangeRate)
    {
        Code = code;
        Name = name;
        Symbol = symbol;
        ExchangeRate = exchangeRate;
    }
    public void Print() => Console.WriteLine($"Код валюты: {Code}, " +
                                             $"Название валюты: {Name}, " +
                                             $"Символ валюты: {Symbol}, " +
                                             $"Курс валюты: {ExchangeRate}");
}