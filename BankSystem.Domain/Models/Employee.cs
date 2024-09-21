namespace BankSystem.Domain.Models;

public class Employee : Person
{
    public Guid EmployeeId { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public int Salary { get; set; }
    public string Contract { get; set; }
        
    public Employee(string name, string surname, string numPassport, int age, string phone, string position, DateTime startDate, int salary)
        : base(name, surname, numPassport, age, phone) // ????? ???????????? ???????? ?????? Person
    {
        EmployeeId = Guid.NewGuid();
        Position = position;
        StartDate = startDate;
        Salary = salary;
        Contract =  string.Empty; //?? ????????? ????????????? ? null (?????????, ??????), ??????????? "??????"
    }
    
    public void Print() => Console.WriteLine($"Name: {Name}, " +
                                             $"Surname: {Surname}, " +
                                             $"Position: {Position}, " +
                                             $"Salary: {Salary}");
}