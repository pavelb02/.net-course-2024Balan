namespace BankSystem.Domain.Models;

public class Employee : Person
{
    public Guid EmployeeId { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public int Salary { get; set; }
    public string Contract { get; set; }

    public Employee(string name, string surname, string numPassport, int age, string phone, string position,
        DateTime startDate, int salary)
        : base(name, surname, numPassport, age, phone)
    {
        EmployeeId = Guid.NewGuid();
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
        return Name == employee.Name &&
               Surname == employee.Surname &&
               NumPassport == employee.NumPassport &&
               Age == employee.Age &&
               Phone == employee.Phone &&
               Position == employee.Position &&
               Salary == employee.Salary &&
               StartDate == employee.StartDate;
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
        return hashCode;
    }

    public override string ToString()
    {
        return $"Name: {Name}, Surname: {Surname}, Position: {Position}, Salary: {Salary}";
    }
}