using System.Globalization;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using System.Text.Json;
using System.Threading;
using BankSystem.App.Dto;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Query;

namespace ExportEntity.Tests;

public class ThreadAndTaskTests
{
    private TestDataGenerator _testDataGenerator = new TestDataGenerator();

    [Fact]
    public void WriteClientsToCsvThreadsTest()
    {
        //Arrange
        var pathToDirectory = Path.Combine("D:", "Программирование", "Dex backend 2024", "Practice",
            ".net-course-2024Balan", "Tool");
        var fullPath = "";
        object locker = new();
        var flag = true;
        var countFile = 0;
        var countThread = 5;
        var countClients = 10;
        var clientsFromFile = new List<ClientDto>();

        //Act
        for (int i = 1; i <= countThread; i++)
        {
            Thread myThread = new(Serialize);
            myThread.Name = $"Поток {i}";
            myThread.Start();
        }

        Thread.Sleep(1000);

        //Assert
        for (int i = 1; i <= countFile; i++)
        {
            var fileName = "clientsCsvThread" + i + ".csv";
            fullPath = Path.Combine(pathToDirectory, fileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Open))
            using (var streamReader = new StreamReader(fileStream))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                clientsFromFile.AddRange(csvReader.GetRecords<ClientDto>().ToList());
            }
        }

        Assert.Equal(countClients * countThread, clientsFromFile.Count);

        void Serialize(object state)
        {
            List<ClientDto> clientsList = _testDataGenerator.GenerateClientsBankList(countClients);

            foreach (var client in clientsList)
            {
                lock (locker)
                {
                    if (flag)
                    {
                        countFile++;
                        string fileName = "clientsCsvThread" + countFile + ".csv";
                        fullPath = Path.Combine(pathToDirectory, fileName);

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        using (var streamWriter = new StreamWriter(fileStream))
                        using (var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                        {
                            writer.WriteHeader<ClientDto>();
                            writer.NextRecord();
                        }

                        flag = false;
                    }

                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Append))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream))
                        {
                            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                            {
                                csvWriter.WriteRecord(client);
                                csvWriter.NextRecord();

                                csvWriter.Flush();

                                long fileStreamSize = fileStream.Length;
                                Console.WriteLine($"Объем данных в файле: {fileStreamSize} б");
                                if (fileStream.Length > 1000)
                                    flag = true;
                            }
                        }
                    }
                }

                Thread.Sleep(10);
            }
        }
    }

    [Fact]
    public void AddMoneyToAccountThreadsTest()
    {
        //Arrange
        object locker = new();
        Account account = new Account();

        //Act
        for (int i = 1; i <= 2; i++)
        {
            Thread myThread = new(AddMoney);
            myThread.Name = $"Поток {i}";
            myThread.Start();
        }

        Thread.Sleep(1000);
        //Assert
        Assert.Equal(2000, account.Amount);

        void AddMoney()
        {
            for (int i = 0; i < 10; i++)
            {
                lock (locker)
                {
                    account.Amount += 100;
                    Console.WriteLine($"{Thread.CurrentThread.Name}: добавил 100");
                }

                Thread.Sleep(10);
            }
        }
    }

    [Fact]
    public void Test7()
    {
        Console.WriteLine("1");
        Print1();
        Console.WriteLine("2");
    }

    private async Task Print1()
    {
        Console.WriteLine("3");
        await Print2();
        Console.WriteLine("4");
    }

    private async Task Print2()
    {
        Console.WriteLine("5");
        Task.Delay(1000);
        Console.WriteLine("6");
    }
}