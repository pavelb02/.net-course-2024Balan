using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace ExportEntity.Tests;

public class ExportServiceTests
{
    private EmployeeStorage _employeeStorage;
    private EmployeeService _employeeService;
    private ClientStorage _clientStorage;
    private ClientService _clientService;
    private CurrencyService _currencyService;
    private CurrencyStorage _currencyStorage;

    public ExportServiceTests()
    {
        _employeeStorage = new EmployeeStorage();
        _employeeService = new EmployeeService(_employeeStorage);
        _currencyStorage = new CurrencyStorage();
        _currencyService = new CurrencyService(_currencyStorage);
        _clientStorage = new ClientStorage();
        _clientService = new ClientService(_clientStorage, _currencyService);
    }
    
    [Fact]
    public void WriteClientsToCsvAndReadFromDbTest()
    {
        //Arrange
        List<Client> clientsFromDb = new (_clientService.FilterClients(new SearchRequest()));
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
    public void ReadClientsFromCsvAndWriteToDbTest()
    {
        //Arrange
        string pathToDirectory = Path.Combine("D:", "Программирование", "Dex backend 2024", "Practice", ".net-course-2024Balan", "Tool");
        string fileName = "clientsCsv.csv";
        ExportService<Client> exportService = new ExportService<Client>(pathToDirectory, fileName);
        List<Client> clientsFromFile = exportService.ReadItemsFromCsv();
        
        //Act
        foreach (var client in clientsFromFile)
        {
            _clientService.AddClient(client, "USD");
        }
        List<Client> clientsFromDb = new List<Client>();
        foreach (var client in clientsFromFile)
        {
            clientsFromDb.Add(_clientService.GetClient(client.Id));
        }

        //Assert
        Assert.Equal(clientsFromFile, clientsFromDb);
    }

    [Fact]
    public void WriteClientsToJsonAndReadClientsFromJsonTest()
    {
        //Arrange
        List<Client> clientsFromDb = new (_clientService.FilterClients(new SearchRequest()));
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
    public void WriteClientToJsonAndReadClientFromJsonTest()
    {
        //Arrange
        List<Client> clientsFromDb = new (_clientService.FilterClients(new SearchRequest()));
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
    public void WriteEmployeesToCsvAndReadFromDbTest()
    {
        //Arrange
        List<Employee> employeesFromDb = new (_employeeService.FilterEmployees(new SearchRequest {PageSize = 5, PageNumber = 1}));
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
    public void ReadEmployeesFromCsvAndWriteToDbTest()
    {
        //Arrange
        var pathToDirectory = Path.Combine("D:", "Программирование","Dex backend 2024", "Practice",".net-course-2024Balan", "Tool");
        var fileName = "employeesCsv.csv";
        ExportService<Employee> exportService = new ExportService<Employee>(pathToDirectory, fileName);
        var employeesFromFile = exportService.ReadItemsFromCsv();
        
        //Act
        _employeeService.AddEmployees(employeesFromFile);
        var employeesFromDb = new List<Employee>();
        foreach (var employee in employeesFromFile)
        {
            employeesFromDb.Add(_employeeService.GetEmployee(employee.Id));
        }

        //Assert
        Assert.Equal(employeesFromFile, employeesFromDb);
    }
    
    [Fact]
    public void WriteEmployeesToJsonAndReadEmployeesFromJsonTest()
    {
        //Arrange
        List<Employee> employeesFromDb = new (_employeeService.FilterEmployees(new SearchRequest{PageSize = 3, PageNumber = 1}));
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
    public void WriteEmployeeToJsonAndReadEmployeeFromJsonTest()
    {
        //Arrange
        List<Employee> employeesFromDb = new (_employeeService.FilterEmployees(new SearchRequest{PageSize = 3, PageNumber = 1}));
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