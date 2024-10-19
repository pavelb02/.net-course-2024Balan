namespace BankSystem.Domain.Models;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string NumPassport { get; set; }
    public string Phone { get; set; }
    public DateTime DateBirthday { get; set; }

    protected Person(string name, string surname, string numPassport, string phone, DateTime dateBirthday)
    {
        Name = name;
        Surname = surname;
        NumPassport = numPassport;
        Phone = phone;
        DateBirthday = dateBirthday.ToUniversalTime();
    }
    protected Person (){}
    public int CalculateAge(DateTime dateBirthday)
    {
        int age = DateTime.Now.Year - dateBirthday.Year;
        
        if (DateTime.Now < dateBirthday.AddYears(age))
        {
            age--;
        }

        return age;
    }
}