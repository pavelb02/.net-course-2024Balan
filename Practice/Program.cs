﻿using System.Diagnostics;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace Practice;

class Program
{
    static void Main()
    {
        // Практическое задание 
        // 1 part
        Employee employee1 = new ("Ivan", "Ivanov", "TY563156", 25,"7894561230", "Manager", DateTime.Now, 5000, new DateTime(2000, 8, 21));
        
        Console.WriteLine(employee1.Contract);
        GenerateContract(employee1);
        Console.WriteLine(employee1.Contract);

        Currency currency1 = new ("USD", "dollar USA", "$", 16);
        Console.WriteLine(currency1.ToString());
        UpdateCurrency(ref currency1);
        Console.WriteLine(currency1.ToString());
        Console.WriteLine();
        // 2 part
        // Лист клиентов банка
        var client = new Client("Dmitriy", "Dmitrov", "YU156489", 27, "7984248948", 10000.50m, new DateTime(1990, 8, 21));
        var clients = new List<Client>
        {
            client,
            new Client("Alexey", "Alexeev", "YY187459", 28, "7897458948", 31500.00m, new DateTime(1995, 8, 21)),
            new Client("Sergey", "Seriev", "HU789654", 26, "7982758728", 120400.60m, new DateTime(1996, 8, 21)),
            new Client("Viktor", "Viktorov", "YE147852", 24, "7278287948", 82000.80m, new DateTime(1999, 8, 21)),
            new Client("Pavel", "Pavlov", "DU369852", 22, "7782827278", 23000.75m, new DateTime(1992, 8, 21))
        };
        // Лист сотрудников
        var employees = new List<Employee>
        {
            new Employee("Ivan", "Ivanov", "TY563156", 29, "7894561230", "Manager", DateTime.Now, 5000, new DateTime(2002, 8, 21)),
            new Employee("Petr", "Petrov", "RT768451", 31, "7782453210", "Engineer", DateTime.Now, 4500, new DateTime(1999, 8, 21))
        };
        // Лист владельцев
        var owners = new List<Employee>
        {
            new Employee("Sergey", "Sidorov", "PL123456", 42, "7765432190", "Owner", DateTime.Now, 0, new DateTime(1993, 8, 21)),
            new Employee("Olga", "Smirnova", "WE987654", 37, "7543217890", "Co-owner", DateTime.Now, 0, new DateTime(1997, 8, 21)),
            new Employee("Ekaterina", "Kuznetsova", "YT789654", 39, "7321765890", "Co-owner", DateTime.Now, 0, new DateTime(2000, 2, 9))
        };
        var bankService = new BankService();
        var salaryOwner= bankService.SalaryCalculate(clients, employees, owners.Count);
        foreach (var owner in owners)
        {
            owner.Salary = salaryOwner;
            Console.WriteLine(owner.ToString());

        }

        Console.WriteLine();
        var clientBank = new Client("Vasiliy", "Vaskov", "KJ197358", 21, "7465426248", 12300.20m, new DateTime(2000, 9, 29));
        Console.WriteLine(clientBank.ToString());
        var employeeBank = bankService.ChangeClientToEmployee(clientBank, 2500);
        Console.WriteLine(employeeBank.ToString());

        // Практическое задание List_Dictionary
        var testDataGenerator = new TestDataGenerator();
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        Console.WriteLine("\n" + clientsBankList.Count + " клиентов банка");

        var clientsBankDictionary = testDataGenerator.GenerateClientsBankDictionary(clientsBankList);
        
        string[] positions = { "Cashier", "Service Specialist", "Counselor", "Manager", "Bank Accountant", "Financial Analyst", "Auditor", "IT specialist" };
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(1000, positions);
        Console.WriteLine(employeesBankList.Count + " сотрудников банка");

        var random = new Random();
        var randomClient = clientsBankList[random.Next(clientsBankList.Count)];
        var sw = new Stopwatch();
        sw.Start();
        var foundListClientPhone = clientsBankList.Find(clientList => clientList.Phone == randomClient.Phone);
        sw.Stop();
        Console.WriteLine($"\nВремя поиска по номеру телефона: \n{sw.Elapsed} List");
        
        sw.Restart();
        var foundDictionaryClientPhone = clientsBankDictionary[randomClient.Phone];
        sw.Stop();
        Console.WriteLine($"{sw.Elapsed} Dictionary");
        
        sw.Restart();
        var foundListClientAge = clientsBankList.Where(clientList => clientList.Age < 35).ToList();
        sw.Stop();
        Console.WriteLine($"\nВремя выборки по возрасту: \n{sw.Elapsed} List");
        
        sw.Restart();
        var foundDictionaryClientAge = clientsBankDictionary.Where(clientDictionary => clientDictionary.Value.Age < 35).ToList();
        sw.Stop();
        Console.WriteLine($"{sw.Elapsed} Dictionary");
        
        sw.Restart();
        var foundListEmployeeMinSalary = employeesBankList.MinBy(employee => employee.Salary);
        sw.Stop();
        Console.WriteLine($"\nВремя поиска сотрудника с минимальной зарплатой: \n{sw.Elapsed} List");
        
        sw.Restart();
        var foundDictionaryLastOrDefault = clientsBankDictionary.LastOrDefault();
        sw.Stop();
        Console.WriteLine($"\nПоиск в Dictionary: \n{sw.Elapsed} LastOrDefault");

        sw.Restart();
        var foundDictionaryKey = clientsBankDictionary[foundDictionaryLastOrDefault.Key];
        sw.Stop();
        Console.WriteLine($"{sw.Elapsed} Key\n");
        
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