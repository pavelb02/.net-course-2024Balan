namespace BankSystem.App.Services;

public class SearchRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string NumPassport { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
}