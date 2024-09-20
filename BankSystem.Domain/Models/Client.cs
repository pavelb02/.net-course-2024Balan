namespace BankSystem.Domain.Models;

class Client : Person
{
    public Guid ClientId { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
}