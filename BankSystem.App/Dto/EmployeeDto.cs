namespace BankSystem.App.Dto;

public class EmployeeDto
{
    public EmployeeDto(string name, string surname, string numPassport, string phone, string position,
        DateTime startDate, int salary, DateTime dateBirthday)
    {
        Position = position;
        StartDate = startDate.ToUniversalTime();;
        Salary = salary;
        Contract = string.Empty;
    }

    public EmployeeDto()
    {
        
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string NumPassport { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public int Salary { get; set; }
    public string Contract { get; set; } = "Contract";
    public DateTime DateBirthday { get; set; }
}