﻿using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace Practice;

class Program
{
    static void Main()
    {
        // 1 part
        Employee employee1 = new ("Ivan", "Ivanov", "TY563156", 25,"7894561230", "Manager", DateTime.Now, 5000);
        
        Console.WriteLine(employee1.Contract);
        GenerateContract(employee1);
        Console.WriteLine(employee1.Contract);

        Currency currency1 = new ("USD", "dollar USA", "$", 16);
        currency1.Print();
        UpdateCurrency(ref currency1);
        currency1.Print();
        Console.WriteLine();
        // 2 part
        // Лист клиентов банка
        var client = new Client("Dmitriy", "Dmitrov", "YU156489", 27, "7984248948", "1782936458", 10000.50m);
        List<Client> clients = new List<Client>
        {
            client,
            new Client("Alexey", "Alexeev", "YY187459", 28, "7897458948", "1789474818", 31500.00m),
            new Client("Sergey", "Seriev", "HU789654", 26, "7982758728", "1782151511", 120400.60m),
            new Client("Viktor", "Viktorov", "YE147852", 24, "7278287948", "1714541458", 82000.80m),
            new Client("Pavel", "Pavlov", "DU369852", 22, "7782827278", "1751578578", 23000.75m)
        };
        // Лист сотрудников
        List<Employee> employees = new List<Employee>
        {
            new Employee("Ivan", "Ivanov", "TY563156", 29, "7894561230", "Manager", DateTime.Now, 5000),
            new Employee("Petr", "Petrov", "RT768451", 31, "7782453210", "Engineer", DateTime.Now, 4500)
        };
        // Лист владельцев
        List<Employee> owners = new List<Employee>
        {
            new Employee("Sergey", "Sidorov", "PL123456", 42, "7765432190", "Owner", DateTime.Now, 0),
            new Employee("Olga", "Smirnova", "WE987654", 37, "7543217890", "Co-owner", DateTime.Now, 0),
            new Employee("Ekaterina", "Kuznetsova", "YT789654", 39, "7321765890", "Co-owner", DateTime.Now, 0)
        };
        var bankService = new BankService();
        var salaryOwner= bankService.SalaryCalculation(clients, employees, owners.Count);
        foreach (var owner in owners)
        {
            owner.Salary = salaryOwner;
            owner.Print();
        }

        Console.WriteLine();
        Client clientBank = new Client("Vasiliy", "Vaskov", "KJ197358", 21, "7465426248", "7568941412", 12300.20m);
        clientBank.Print();
        Employee employeeBank = bankService.ChangeClientToEmployee(clientBank);
        employeeBank.Print();
        Console.ReadKey();
    }


// Метод для обновления контракта
    static void GenerateContract(Employee employee)
    {
        employee.Contract =
            $"Contract for {employee.Name} {employee.Surname}, Position: {employee.Position}, Salary: {employee.Salary:C}"; // "C" для вывода значка валюты
    }
// Метод для изменения структуры Currency
    static void UpdateCurrency(ref Currency currency)
    {
        currency.Code = "EUR";
        currency.Name = "euro";
        currency.Symbol = "€";
        currency.ExchangeRate = 19;
    }
}