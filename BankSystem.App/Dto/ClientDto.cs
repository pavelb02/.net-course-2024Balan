using BankSystem.Domain.Models;

namespace BankSystem.App.Dto;

public class ClientDto
{
    public ClientDto(string name, string surname, string numPassport, string phone, DateTime dateBirthday)
    {
        Name = name;
        Surname = surname;
        NumPassport = numPassport;
        Phone = phone;
        DateBirthday = dateBirthday;
    }

    public ClientDto()
    {
        
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string NumPassport { get; set; }
    public string Phone { get; set; }
    public DateTime DateBirthday { get; set; }
    public ICollection<Account> AccountsClient { get; set; } = new List<Account>();
}