namespace BankSystem.Domain.Models;

public class Employee : Person
{
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public int Salary { get; set; }
    public string Contract { get; set; } = "Contract";

    public Employee(string name, string surname, string numPassport, string phone, string position,
        DateTime startDate, int salary, DateTime dateBirthday)
        : base(name, surname, numPassport, phone, dateBirthday)
    {
        Position = position;
        StartDate = startDate;
        Salary = salary;
        Contract = string.Empty;
    }

    public Employee() { }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is Employee))
            return false;
        var employee = (Employee)obj;
        return NumPassport == employee.NumPassport &&
               Phone == employee.Phone;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, NumPassport);
    }

    public override string ToString()
    {
        return $"Name: {Name}, Surname: {Surname}, Position: {Position}, Salary: {Salary}";
    }
}