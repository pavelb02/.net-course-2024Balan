using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class BankService
{
    public int SalaryCalculate(List<Client> clients, List<Employee> employees, int countOwner)
    {
        decimal profit = 0;
        foreach (var client in clients)
        {
            profit = client.AccountsClient.Sum(a => 0.2m * a.Amount);
        }
        var costs = employees.Sum(employee => employee.Salary);

        if (countOwner <= 0)
            throw new ArgumentException("У банка должен быть хотя бы один владелец.");
        var salaryOwner = (int)((profit - costs) / countOwner);
        return salaryOwner;
    }

    public Employee ChangeClientToEmployee(Client client, int internSalary)
    {
        Employee employee = new(client.Name, client.Surname, client.NumPassport, client.Phone, "Intern",
            DateTime.Now, internSalary, new DateTime(2002, 7, 20));
        return employee;
    }
}