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

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is Client))
            return false;
        var client = (Client)obj;

        return Name == client.Name &&
               Surname == client.Surname &&
               NumPassport == client.NumPassport &&
               Age == client.Age &&
               Phone == client.Phone &&
               AccountNumber == client.AccountNumber &&
               Balance == client.Balance;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Surname, NumPassport, Age, Phone, AccountNumber, Balance);
    }
    public override string ToString()
    {
        return $"Name: {Name}, Surname: {Surname}, Position: AccountNumber: {AccountNumber}, Balance: {Balance} Phone: {Phone}";
    }
}