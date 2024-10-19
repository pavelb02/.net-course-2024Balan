using BankSystem.Domain.Models;
using CsvHelper.Configuration;

namespace ExportEntity;

public class ClientMap : ClassMap<Client>
{
    public ClientMap()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.Name).Name("Name");
        Map(m => m.Surname).Name("Surname");
        Map(m => m.NumPassport).Name("NumPassport");
        Map(m => m.Phone).Name("Phone");
        Map(m => m.DateBirthday).Name("DateBirthday");
    }
}