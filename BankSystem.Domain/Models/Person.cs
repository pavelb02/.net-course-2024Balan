namespace BankSystem.Domain.Models;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string NumPassport { get; set; }
    public int Age { get; private set; }
    public string Phone { get; set; }
    public DateTime DateBirthday { get; set; }

    protected Person(string name, string surname, string numPassport, int age, string phone, DateTime dateBirthday)
    {
        Name = name;
        Surname = surname;
        NumPassport = numPassport;
        Age = CalculateAge(dateBirthday);
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