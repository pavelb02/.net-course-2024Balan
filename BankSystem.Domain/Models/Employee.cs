namespace BankSystem.Domain.Models;

class Employee : Person
{
    public Guid EmployeeId { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public decimal Salary { get; set; }
    public string Contract { get; set; }
        
    public Employee(string name, string surname, string position, DateTime startDate, decimal salary)
    {
        EmployeeId = Guid.NewGuid();
        Name = name;
        Surname = surname;
        Position = position;
        StartDate = startDate;
        Salary = salary;
        Contract = string.Empty; //????? ?? ????? ???? ? null (?????????????? ??????????, ??????), ??????????? "??????"
    }
}