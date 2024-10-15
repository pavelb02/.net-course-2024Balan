namespace BankSystem.Domain.Models;

public class Account
{
    public Account(decimal amount, Guid clientId, Client client)
    {
        Currency = new Currency("USD", "Dollar", "$", 16.3m);;
        Amount = amount;
        ClientId = clientId;
        Client = client;
    }

    public Guid Id { get; set; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public Guid ClientId { get; set; }
    public Client Client { get; set; }
}