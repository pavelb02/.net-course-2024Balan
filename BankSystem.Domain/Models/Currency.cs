namespace BankSystem.Domain.Models;

public struct Currency
{
    public string Code { get; set; }      // Код валюьы (USD, EUR, RUB)
    public string Name { get; set; }      // Название валюты (доллар США, евро, российский рубль)
    public string Symbol { get; set; }    // Символ валюты ($, €, ₽)
    public decimal ExchangeRate { get; set; } // Курс валюты
}