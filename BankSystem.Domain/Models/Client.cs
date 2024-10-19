namespace BankSystem.Domain.Models;

public class Client : Person
{
    public ICollection<Account> AccountsClient { get; set; } = new List<Account>();
    public Client(string name, string surname, string numPassport, string phone, DateTime dateBirthday)
        : base(name, surname, numPassport, phone, dateBirthday)
    {
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
        return HashCode.Combine(Id, NumPassport);
    }
    public override string ToString()
    {
        return $"Name: {Name}, Surname: {Surname}, Position: Phone: {Phone}";
    }
}