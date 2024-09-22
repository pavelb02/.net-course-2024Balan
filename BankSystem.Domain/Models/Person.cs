namespace BankSystem.Domain.Models;

public class Person
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string NumPassport { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; }

    protected Person(string name, string surname, string numPassport, int age, string phone)
    {
        Name = name;
        Surname = surname;
        NumPassport = numPassport;
        Age = age;
        Phone = phone;
    }
}