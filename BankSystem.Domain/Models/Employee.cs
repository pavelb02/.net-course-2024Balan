namespace BankSystem.Domain.Models;

public class Employee : Person
{
    public Guid EmployeeId { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public int Salary { get; set; }
    public string Contract { get; set; }
        
    public Employee(string name, string surname, string numPassport, int age, string phone, string position, DateTime startDate, int salary)
        : base(name, surname, numPassport, age, phone) 
    {
        EmployeeId = Guid.NewGuid();
        Position = position;
        StartDate = startDate;
        Salary = salary;
        Contract =  string.Empty; 
    }

    public Employee() { }

    public override string ToString()
    {
        return $"Name: {Name}, Surname: {Surname}, Position: {Position}, Salary: {Salary}";
    }
}