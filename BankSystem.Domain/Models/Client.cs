namespace BankSystem.Domain.Models;

public class Client : Person
{
    public Guid ClientId { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public Client(string name, string surname, string numPassport, int age, string phone, string accountNumber, decimal balance, DateTime dateBirthday)
        : base(name, surname, numPassport, age, phone, dateBirthday)
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

        return NumPassport == client.NumPassport &&
               Phone == client.Phone;

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