using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Mapping;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests;

public class EquivalenceTests
{
    TestDataGenerator _testDataGenerator = new();
    private ClientStorage _clientStorage;
    private ClientService _clientService;
    private CurrencyService _currencyService;
    private EmployeeStorage _employeeStorage;
    private CurrencyStorage _currencyStorage;
    private EmployeeService _employeeService;
    private IMapper _mapper;

    public EquivalenceTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ClientProfile>(); 
        });
    
        _mapper = mapperConfig.CreateMapper();
        _currencyStorage = new CurrencyStorage();
        _currencyService = new CurrencyService(_currencyStorage);
        _clientStorage = new ClientStorage();
        _clientService = new ClientService(_clientStorage, _currencyService, _mapper);
        _employeeStorage = new EmployeeStorage();
        _employeeService = new EmployeeService(_employeeStorage, _mapper);
    }

    string[] positions =
    {
        "Cashier", "Service Specialist", "Counselor", "Manager", "Bank Accountant", "Financial Analyst", "Auditor",
        "IT specialist"
    };

    Currency[] currencies =
    {
        new("USD", "Dollar USA", "$", 16.3m),
        new("EUR", "Euro", "€", 18.6m),
        new("RUP", "Russian ruble", "₽", 0.185m)
    };

    private const string _defaultCurrencyCode = "USD";

    [Fact]
    public void GetHashCodeNecessityPositiveTest()
    {
        //Arrange
        var clientsBankListDto = _testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            _testDataGenerator.GenerateClientsBankDictionaryAccount(clientsBankListDto, currencies);
        var client = clientsBankDictionaryAccount.Keys.First();
        var newClient = new ClientDto(client.Name, client.Surname, client.NumPassport, client.Phone,
            client.DateBirthday);
        //Act
        AccountDto result = clientsBankDictionaryAccount[newClient];
        //Assert
        Assert.Equal(result, clientsBankDictionaryAccount[client]);
    }

    [Fact]
    public void GetHashCodeNecessityPositiveMultiAccountTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount = _testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        var client = clientsBankDictionaryAccount.Keys.First();
        var newClient = new ClientDto(client.Name, client.Surname, client.NumPassport, client.Phone, client.DateBirthday);
        //Act
        AccountDto[] result = clientsBankDictionaryAccount[newClient];
        //Assert
        Assert.Equal(result, clientsBankDictionaryAccount[client]);
    }

    [Fact]
    public void GetHashCodeNecessityPositiveListTest()
    {
        //Arrange
        var employeesBankList = _testDataGenerator.GenerateEmployeesBankList(10, positions);
        var employee = employeesBankList.First();
        var newEmployee = new EmployeeDto(employee.Name, employee.Surname, employee.NumPassport,
            employee.Phone, employee.Position, employee.StartDate, employee.Salary, employee.DateBirthday);
        //Act
        bool result = employeesBankList.Contains(newEmployee);
        //Assert
        Assert.Equal(result, employeesBankList.Contains(employeesBankList.First()));
    }

    [Fact]
    public async Task AddClientAsyncPositiveListTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(13);
        var clientsBankListDto = _mapper.Map<List<ClientDto>>(clientsBankList);
        foreach (var client in clientsBankListDto)
        {
            await _clientService.AddClientAsync(client, _defaultCurrencyCode);
        }

        //Act
        var clientOne = await _clientService.FilterClientsAsync(new SearchRequest{NumPassport = clientsBankList.First().NumPassport});
        //Assert
        Assert.Equal(clientsBankList.First(), clientOne.First());
    }

    [Fact]
    public async Task DeleteAllClientsAsyncPositiveListTest()
    {
        //Arrange
        var clientsBankListDto = await _clientService.FilterClientsAsync(new SearchRequest ());
        var clientsBankList = _mapper.Map<List<Client>>(clientsBankListDto);
        //Act
        foreach (var client in clientsBankList)
        {
            await _clientService.DeleteClientAsync(client.Id);
        }
        
        //Assert
        var clientsBankListNow = await _clientService.FilterClientsAsync(new SearchRequest ());
        Assert.Equal(0, clientsBankListNow.Count);
    }

    [Fact]
    public async Task AddClientAsyncNegativeListTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(1);
        clientsBankList.First().Name = "";
        var clientDto = _mapper.Map<ClientDto>(clientsBankList.First());
        //Act
        await _clientService.AddClientAsync(clientDto, _defaultCurrencyCode);
        //Assert
        await Assert.ThrowsAsync<Exception>(async () =>
            await _clientService.AddClientAsync(clientDto, _defaultCurrencyCode));

    }

    [Fact]
    public async Task AddAccountAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(1);
        var clientDto = _mapper.Map<ClientDto>(clientsBankList.First());
        await _clientService.AddClientAsync(clientDto, _defaultCurrencyCode);
        await _clientService.AddAccountAsync(clientsBankList.First().Id, "USD");
        //Act
        var clientsDto = await _clientService.GetClientAsync(clientsBankList.First().Id);
        var clients = _mapper.Map<List<Client>>(clientsDto);
        var result = clients.First().AccountsClient.Count;
        //Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateClientAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(1);
        var clientDto = _mapper.Map<ClientDto>(clientsBankList.First());
        await _clientService.AddClientAsync(clientDto, _defaultCurrencyCode);
        var newClient = new ClientDto
        {
            Name = "Pavlik",
            Surname = "Balan",
            Phone = "+37368523915",
            NumPassport = "545464546",
        };
        await _clientService.UpdateClientAsync(clientsBankList.First().Id, newClient);
        //Act
        var result = await _clientService.GetClientAsync(clientsBankList.First().Id);
        //Assert
        Assert.Equal(result, newClient);
    }

    [Fact]
    public async Task FilterClientAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(1);
        var clientDto = _mapper.Map<ClientDto>(clientsBankList.First());
        await _clientService.AddClientAsync(clientDto, _defaultCurrencyCode);
        var searchRequest = new SearchRequest { NumPassport = clientsBankList[0].NumPassport };
        //Act
        var filteredClients = await _clientService.FilterClientsAsync(searchRequest);
        // Assert
        Assert.Single(filteredClients);
        Assert.Equal(clientsBankList[0].NumPassport, filteredClients.First().NumPassport);
    }
    
    [Fact]
    public async Task DebitAsyncPositiveTest()
    {
        //Arrange & Act
        var pageSize = 5;
        var iterationCount = 3;
        var withdrawalAmount = 10;
        var currencyCode = "USD";

        var currencyId = await _currencyService.GetCurrencyAsync(currencyCode);
        var clientBeforeDto = await _clientService.FilterClientsAsync(new SearchRequest { PageNumber = 1, PageSize = 1 });
        var clientBefore = _mapper.Map<List<Client>>(clientBeforeDto);
        var amountBefore = clientBefore.First().AccountsClient.FirstOrDefault(a => a.CurrencyId == currencyId)!.Amount;
        
        for (int i = 0; i < iterationCount; i++)
        {
            var pageNumber = 1;
            var flag = true;
            while (flag)
            {
                var clients = await _clientStorage.GetCollectionAsync(new SearchRequest
                    { PageSize = pageSize, PageNumber = pageNumber });
                if (clients.Count < pageSize)
                    flag = false;

                foreach (var client in clients)
                {
                    var task = _clientService.Debit(new WithdrawalRequest
                    {
                        ClientId = client.Id,
                        WithdrawalAmount = withdrawalAmount,
                        CurrencyCode = currencyCode
                    });

                    await task; 
                }

                pageNumber++;
            }
        }

        // Assert
        var clientAfterDto = await _clientService.FilterClientsAsync(new SearchRequest { PageNumber = 1, PageSize = 1 });
        var clientAfter = _mapper.Map<List<Client>>(clientAfterDto);
        var amountAfter = clientAfter.First().AccountsClient.FirstOrDefault(a => a.CurrencyId == currencyId);
        Assert.Equal(amountBefore - iterationCount * withdrawalAmount, amountAfter.Amount);
    }

    [Fact]
    public async Task AddEmployeeAsyncPositiveListTest()
    {
        //Arrange
        var employeesBankListDto = _testDataGenerator.GenerateEmployeesBankList(3, positions);
        foreach (var employeeDto in employeesBankListDto)
        {
            await _employeeService.AddEmployeeAsync(employeeDto);
        }
        var employee = _mapper.Map<Employee>(employeesBankListDto.First());
        //Act
        var employeeFromBd = await _employeeStorage.GetCollectionAsync(new SearchRequest {NumPassport = employee.NumPassport});
        //Assert
        Assert.Equal(employee, employeeFromBd.First());
    }

    [Fact]
    public async Task FilterEmployeeAsyncPositiveTest()
    {
        //Arrange
        var employeesBankListDto = _testDataGenerator.GenerateEmployeesBankList(3, positions);
        foreach (var employeeDto in employeesBankListDto)
        {
            await _employeeService.AddEmployeeAsync(employeeDto);
        }
        var searchRequest = new SearchRequest { NumPassport = employeesBankListDto[0].NumPassport };
        //Act
        var filteredEmployeesDto = await _employeeService.FilterEmployeesAsync(searchRequest);
        //Assert
        Assert.Equal(employeesBankListDto[0].NumPassport, filteredEmployeesDto.First().NumPassport);
    }

    [Fact]
    public async Task DeleteEmployeeAsyncPositiveListTest()
    {
        //Arrange
        var employeesBankList = await _employeeService.FilterEmployeesAsync(new SearchRequest { });
        //Act
        await _employeeService.DeleteEmployeeAsync(employeesBankList.First().Id);
        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _employeeStorage.GetByIdAsync(employeesBankList.First().Id));
    }
}