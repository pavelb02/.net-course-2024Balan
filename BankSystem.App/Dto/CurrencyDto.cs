using BankSystem.Domain.Models;

namespace BankSystem.App.Dto;

public class CurrencyDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } // Код валюты (USD, EUR, RUB)
    public string Name { get; set; } // Название валюты (доллар США, евро, российский рубль)
    public string Symbol { get; set; } // Символ валюты ($, €, ₽)
    public decimal ExchangeRate { get; set; } // Стоимость валюты
    public ICollection<Account> AccountsCurrency { get; set; }

    public CurrencyDto(string code, string name, string symbol, decimal exchangeRate)
    {
        Code = code;
        Name = name;
        Symbol = symbol;
        ExchangeRate = exchangeRate;
    }

    public CurrencyDto()
    {
        
    }
}