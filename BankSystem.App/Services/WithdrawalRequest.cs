namespace BankSystem.App.Services;

public class WithdrawalRequest
{
    public Guid ClientId { get; set; }
    public decimal WithdrawalAmount { get; set; }
    public string CurrencyCode { get; set; }
}