namespace BankSystem.App.Exeptions;

public class ClientNotFoundException : Exception
{
    public ClientNotFoundException() : base("Клиент не найден.") { }
}