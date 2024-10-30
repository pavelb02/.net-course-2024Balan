using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace ExportEntity.Tests;

public class ExportServiceTests
{
    private EmployeeService _employeeService;
    private ClientService _clientService;

    public ExportServiceTests()
    {
        var employeeStorage = new EmployeeStorage();
        _employeeService = new EmployeeService(employeeStorage);
        var currencyStorage = new CurrencyStorage();
        var currencyService = new CurrencyService(currencyStorage);
        var clientStorage = new ClientStorage();
        _clientService = new ClientService(clientStorage, currencyService);
    }
    
    [Fact]
    public async Task WriteClientsToCsvAndReadFromDbTest()
    {
        //Arrange
        List<Client> clientsFromDb = new (await _clientService.FilterClientsAsync(new SearchRequest()));
        string pathToDirectory = Path.Combine("D:", "Программирование","Dex backend 2024", "Practice",".net-course-2024Balan", "Tool");
        string fileName = "clientsCsv.csv";
        ExportService<Client> exportService = new ExportService<Client>(pathToDirectory, fileName);
        
        //Act
        exportService.WriteItemsToCsv(clientsFromDb);
        List<Client> clientsFromFile = exportService.ReadItemsFromCsv();
        
        //Assert
        Assert.Equal(clientsFromDb, clientsFromFile);
    }
    
    [Fact]
    public async Task ReadClientsFromCsvAndWriteToDbTest()
    {
        //Arrange
        string pathToDirectory = Path.Combine("D:", "Программирование", "Dex backend 2024", "Practice", ".net-course-2024Balan", "Tool");
        string fileName = "clientsCsv.csv";
        ExportService<Client> exportService = new ExportService<Client>(pathToDirectory, fileName);
        List<Client> clientsFromFile = exportService.ReadItemsFromCsv();
        
        //Act
        foreach (var client in clientsFromFile)
        {
            await _clientService.AddClientAsync(client, "USD");
        }
        List<Client> clientsFromDb = new List<Client>();
        foreach (var client in clientsFromFile)
        {
            clientsFromDb.Add(await _clientService.GetClientAsync(client.Id));
        }

        //Assert
        Assert.Equal(clientsFromFile, clientsFromDb);
    }

    [Fact]
    public async Task WriteClientsToJsonAndReadClientsFromJsonTest()
    {
        //Arrange
        List<Client> clientsFromDb = new (await _clientService.FilterClientsAsync(new SearchRequest()));
        var pathToDirectory = Path.Combine("D:", "Программирование", "Dex backend 2024", "Practice", ".net-course-2024Balan", "Tool");
        var fileName = "clientsJson.json";
        var fullPath = Path.Combine(pathToDirectory, fileName);
        ExportService<Client> exportService = new ExportService<Client>();
        
        //Act
        exportService.WriteToJson(clientsFromDb, fullPath);
        var clientsFromFile = exportService.ReadItemsFromJson(fullPath);

        //Assert
        Assert.Equal(clientsFromDb, clientsFromFile);
    }
    
    [Fact]
    public async Task WriteClientToJsonAndReadClientFromJsonTest()
    {
        //Arrange
        List<Client> clientsFromDb = new (await _clientService.FilterClientsAsync(new SearchRequest()));
        var pathToDirectory = Path.Combine("D:", "Программирование", "Dex backend 2024", "Practice", ".net-course-2024Balan", "Tool");
        var fileName = "clientJson.json";
        var fullPath = Path.Combine(pathToDirectory, fileName);
        ExportService<Client> exportService = new ExportService<Client>();
        
        //Act
        exportService.WriteToJson(clientsFromDb.First(), fullPath);
        var clientFromFile = exportService.ReadItemFromJson(fullPath);

        //Assert
        Assert.Equal(clientsFromDb.First(), clientFromFile);
    }
    
    [Fact]
    public async Task WriteEmployeesToCsvAndReadFromDbTest()
    {
        //Arrange
        List<Employee> employeesFromDb = new (await _employeeService.FilterEmployeesAsync(new SearchRequest {PageSize = 5, PageNumber = 1}));
        var pathToDirectory = Path.Combine("D:", "Программирование","Dex backend 2024", "Practice",".net-course-2024Balan", "Tool");
        var fileName = "employeesCsv.csv";
        ExportService<Employee> exportService = new ExportService<Employee>(pathToDirectory, fileName);
        
        //Act
        exportService.WriteItemsToCsv(employeesFromDb);
        var employeesFromFile = exportService.ReadItemsFromCsv();
        
        //Assert
        Assert.Equal(employeesFromDb, employeesFromFile);
    }
    
    [Fact]
    public async Task ReadEmployeesFromCsvAndWriteToDbTest()
    {
        //Arrange
        var pathToDirectory = Path.Combine("D:", "Программирование","Dex backend 2024", "Practice",".net-course-2024Balan", "Tool");
        var fileName = "employeesCsv.csv";
        ExportService<Employee> exportService = new ExportService<Employee>(pathToDirectory, fileName);
        var employeesFromFile = exportService.ReadItemsFromCsv();
        
        //Act
        await _employeeService.AddEmployeesAsync(employeesFromFile);
        var employeesFromDb = new List<Employee>();
        foreach (var employee in employeesFromFile)
        {
            employeesFromDb.Add(await _employeeService.GetEmployeeAsync(employee.Id));
        }

        //Assert
        Assert.Equal(employeesFromFile, employeesFromDb);
    }
    
    [Fact]
    public async Task WriteEmployeesToJsonAndReadEmployeesFromJsonTest()
    {
        //Arrange
        List<Employee> employeesFromDb = new (await _employeeService.FilterEmployeesAsync(new SearchRequest{PageSize = 3, PageNumber = 1}));
        var pathToDirectory = Path.Combine("D:", "Программирование", "Dex backend 2024", "Practice", ".net-course-2024Balan", "Tool");
        var fileName = "employeesJson.json";
        var fullPath = Path.Combine(pathToDirectory, fileName);
        ExportService<Employee> exportService = new ExportService<Employee>();
        
        //Act
        exportService.WriteToJson(employeesFromDb, fullPath);
        var employeesFromFile = exportService.ReadItemsFromJson(fullPath);

        //Assert
        Assert.Equal(employeesFromDb, employeesFromFile);
    }
    
    [Fact]
    public async Task WriteEmployeeToJsonAndReadEmployeeFromJsonTest()
    {
        //Arrange
        List<Employee> employeesFromDb = new (await _employeeService.FilterEmployeesAsync(new SearchRequest{PageSize = 3, PageNumber = 1}));
        var pathToDirectory = Path.Combine("D:", "Программирование", "Dex backend 2024", "Practice", ".net-course-2024Balan", "Tool");
        var fileName = "employeeJson.json";
        var fullPath = Path.Combine(pathToDirectory, fileName);
        ExportService<Employee> exportService = new ExportService<Employee>();
        
        //Act
        exportService.WriteToJson(employeesFromDb.First(), fullPath);
        var employeeFromFile = exportService.ReadItemFromJson(fullPath);

        //Assert
        Assert.Equal(employeesFromDb.First(), employeeFromFile);
    }
}