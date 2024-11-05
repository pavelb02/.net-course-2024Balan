using BankSystem.Domain.Models;

namespace BankSystem.App.Dto;

public class AccountDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Guid ClientId { get; set; }
    public Client Client { get; set; }
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }

    public AccountDto(Guid clientId, Guid currencyId)
    {
        ClientId = clientId;
        CurrencyId = currencyId;
    }

    public AccountDto()
    {
        
    }
}