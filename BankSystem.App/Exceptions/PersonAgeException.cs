namespace BankSystem.App.Exeptions;

public class PersonAgeException : Exception
{
    public int Value { get;}
    public PersonAgeException(int val)
        : base($"Человек должен быть старше 18 лет. Текущий возраст: {val} лет.")
    {
        Value = val;
    }
}