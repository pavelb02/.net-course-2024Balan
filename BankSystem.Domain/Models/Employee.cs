namespace BankSystem.Domain.Models;

public class Employee : Person
{
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public int Salary { get; set; }
    public string Contract { get; set; }

    public Employee(string name, string surname, string numPassport, int age, string phone, string position,
        DateTime startDate, int salary, DateTime dateBirthday)
        : base(name, surname, numPassport, age, phone, dateBirthday)
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
        int hashCode = Name?.GetHashCode() ?? 0;
        hashCode = (hashCode * 31) ^ (Surname?.GetHashCode() ?? 0);
        hashCode = (hashCode * 31) ^ (NumPassport?.GetHashCode() ?? 0);
        hashCode = (hashCode * 31) ^ Age.GetHashCode();
        hashCode = (hashCode * 31) ^ (Phone?.GetHashCode() ?? 0);
        hashCode = (hashCode * 31) ^ (Position?.GetHashCode() ?? 0);
        hashCode = (hashCode * 31) ^ Salary.GetHashCode();
        hashCode = (hashCode * 31) ^ StartDate.GetHashCode();
        hashCode = (hashCode * 31) ^ DateBirthday.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        return $"Name: {Name}, Surname: {Surname}, Position: {Position}, Salary: {Salary}";
    }
}