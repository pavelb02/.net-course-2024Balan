namespace BankSystem.Domain.Models;

public class Account
{
    public Account(decimal amount, Guid clientId, Client client, Guid currencyId)
    {
        Currency = new Currency("USD", "Dollar", "$", 16.3m);;
        Amount = amount;
        ClientId = clientId;
        Client = client;
        CurrencyId = currencyId;
    }
    public Account() { }

    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Guid ClientId { get; set; }
    public Client Client { get; set; }
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    
}