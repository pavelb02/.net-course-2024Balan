namespace BankSystem.App.Services;

public class ConversionCurrencyRequest
{
    public string CurrencyFrom { get; set; }
    public string CurrencyTo { get; set; }
    public decimal Amount { get; set; } = 1;
}