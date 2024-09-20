using BankSystem.Domain.Models;

class Program
{
    static void Main(string[] args)
    {
        Employee employee1 = new ("Ivan", "Ivanov", "Manager", DateTime.Now, 5000);
        
        Console.WriteLine(employee1.Contract);
        GenerateContract(employee1);
        Console.WriteLine(employee1.Contract);

        Currency currency1 = new ("USD", "dollar USA", "$", 16);
        currency1.Print();
        UpdateCurrency(ref currency1);
        currency1.Print();

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