namespace BankSystem.Domain.Models;

public class Client : Person
{
    public Guid ClientId { get; set; }
    public decimal Balance { get; set; }
    public ICollection<Account> AccountsClient { get; set; }
    public Client(string name, string surname, string numPassport, int age, string phone, decimal balance, DateTime dateBirthday,  Guid currencyId)
        : base(name, surname, numPassport, age, phone, dateBirthday)
    {
        Balance = balance;
        AccountsClient = new List<Account>
        {
            new Account(0, ClientId, this, currencyId)
        };
    }
    public Client(string name, string surname, string numPassport, int age, string phone, decimal balance, DateTime dateBirthday)
        : base(name, surname, numPassport, age, phone, dateBirthday)
    {
        Balance = balance;
    }

    public Client() { }
    public Client(Guid clientId)
    {
        Id = clientId;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is Client))
            return false;
        var client = (Client)obj;

        return NumPassport == client.NumPassport &&
               Phone == client.Phone;

    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Surname, NumPassport, Age, Phone, Balance);
    }
    public override string ToString()
    {
        return $"Name: {Name}, Surname: {Surname}, Position: Balance: {Balance} Phone: {Phone}";
    }
}