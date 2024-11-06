using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Mapping;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace ExportEntity.Tests;

public class ExportServiceTests
{
    private EmployeeService _employeeService;
    private ClientService _clientService;
    private IMapper _mapper;
    CancellationToken cancellationToken = CancellationToken.None;

    public ExportServiceTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ClientProfile>(); 
        });
    
        _mapper = mapperConfig.CreateMapper();
        var employeeStorage = new EmployeeStorage();
        _employeeService = new EmployeeService(employeeStorage, _mapper);
        var currencyStorage = new CurrencyStorage();
        var currencyService = new CurrencyService(currencyStorage, _mapper);
        var clientStorage = new ClientStorage();
        _clientService = new ClientService(clientStorage, currencyService, _mapper);
    }
    
    [Fact]
    public async Task WriteClientsToCsvAndReadFromDbTest()
    {
        //Arrange
        List<Client> clientsFromDb = new (_mapper.Map<List<Client>>(await _clientService.FilterClientsAsync(new SearchRequest(), cancellationToken)));
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
        var clientsFromFileDto = _mapper.Map<List<ClientDto>>(clientsFromFile);
        
        //Act
        foreach (var client in clientsFromFileDto)
        {
            await _clientService.AddClientAsync(client, "USD", cancellationToken);
        }
        List<Client> clientsFromDb = new List<Client>();
        foreach (var client in clientsFromFile)
        {
            clientsFromDb.Add(_mapper.Map<Client>(await _clientService.GetClientAsync(client.Id, cancellationToken)));
        }

        //Assert
        Assert.Equal(clientsFromFile, clientsFromDb);
    }

    [Fact]
    public async Task WriteClientsToJsonAndReadClientsFromJsonTest()
    {
        //Arrange
        List<ClientDto> clientsFromDbDto = new (await _clientService.FilterClientsAsync(new SearchRequest(), cancellationToken));
        var clientsFromDb = _mapper.Map<List<Client>>(clientsFromDbDto);
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
        List<ClientDto> clientsFromDbDto = new (await _clientService.FilterClientsAsync(new SearchRequest(), cancellationToken));
        var clientsFromDb = _mapper.Map<List<Client>>(clientsFromDbDto);
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
        List<EmployeeDto> employeesFromDbDto = new (await _employeeService.FilterEmployeesAsync(new SearchRequest {PageSize = 5, PageNumber = 1}, cancellationToken));
        var employeesFromDb = _mapper.Map<List<Employee>>(employeesFromDbDto);
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
        var employeesFromFileDto = _mapper.Map<List<EmployeeDto>>(employeesFromFile);
        
        //Act
        foreach (var employeeDto in employeesFromFileDto)
        {
            await _employeeService.AddEmployeeAsync(employeeDto, cancellationToken);
        }
        var employeesFromDbDto = new List<EmployeeDto>();
        foreach (var employee in employeesFromFile)
        {
            employeesFromDbDto.Add(await _employeeService.GetEmployeeAsync(employee.Id, cancellationToken));
        }

        //Assert
        Assert.Equal(employeesFromFileDto, employeesFromDbDto);
    }
    
    [Fact]
    public async Task WriteEmployeesToJsonAndReadEmployeesFromJsonTest()
    {
        //Arrange
        List<EmployeeDto> employeesFromDbDto = new (await _employeeService.FilterEmployeesAsync(new SearchRequest{PageSize = 3, PageNumber = 1}, cancellationToken));
        var employeesFromDb = _mapper.Map<List<Employee>>(employeesFromDbDto);
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
        List<EmployeeDto> employeesFromDbDto = new (await _employeeService.FilterEmployeesAsync(new SearchRequest{PageSize = 3, PageNumber = 1}, cancellationToken));
        var employeesFromDb = _mapper.Map<List<Employee>>(employeesFromDbDto);
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