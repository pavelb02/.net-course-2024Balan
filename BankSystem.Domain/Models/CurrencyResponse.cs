namespace BankSystem.Domain.Models;

public class CurrencyResponse
{
    public int Error { get; set; }
    public string ErrorMessage { get; set; }
    public decimal Amount { get; set; }
}