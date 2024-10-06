namespace BankSystem.App.Exeptions;

public class CurrencyNotFoundException : Exception
{
    public string CurrencyCode { get; }

    public CurrencyNotFoundException(string currencyCode)
        : base($"Валюта с кодом '{currencyCode}' не найдена.")
    {
        CurrencyCode = currencyCode;
    }
}