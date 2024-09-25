namespace BankSystem.Domain.Models;

public class Client : Person
{
    public Guid ClientId { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public Client(string name, string surname, string numPassport, int age, string phone, string accountNumber, decimal balance)
        : base(name, surname, numPassport, age, phone)
    {
        ClientId = Guid.NewGuid();
        AccountNumber = accountNumber;
        Balance = balance;
    }

    public Client() { }

    public override string ToString()
    {
        return $"Name: {Name}, Surname: {Surname}, Position: AccountNumber: {AccountNumber}, Balance: {Balance} Phone: {Phone}";
    }
}