using System.Reflection.Metadata.Ecma335;

namespace BankSystem.Domain.Models;

public class Currency
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } // Код валюты (USD, EUR, RUB)
    public string Name { get; set; } // Название валюты (доллар США, евро, российский рубль)
    public string Symbol { get; set; } // Символ валюты ($, €, ₽)
    public decimal ExchangeRate { get; set; } // Стоимость валюты
    public ICollection<Account> AccountsCurrency { get; set; }

    public Currency(string code, string name, string symbol, decimal exchangeRate)
    {
        Code = code;
        Name = name;
        Symbol = symbol;
        ExchangeRate = exchangeRate;
    }
    // Переопределение метода ToString
    public override string ToString()
    {
        return $"Код валюты: {Code}, Название валюты: {Name}, Символ валюты: {Symbol}, Стоимость валюты: {ExchangeRate}";
    }
    
}