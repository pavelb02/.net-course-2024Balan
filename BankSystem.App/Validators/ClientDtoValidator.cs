using BankSystem.App.Dto;
using FluentValidation;

namespace BankSystem.App.Validators;

public class ClientDtoValidator : AbstractValidator<ClientDto>
{
    public ClientDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Имя клиента не может быть пустым.")
            .Length(2, 20)
            .WithMessage("Имя клиента должно содержать от 2 до 20 символов.")
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Имя клиента может содержать только буквы.");
        
        RuleFor(c => c.Surname)
            .NotEmpty()
            .WithMessage("Фамилия клиента не может быть пустой.")
            .Length(2, 20)
            .WithMessage("Фамилия клиента должна содержать от 2 до 20 символов.")
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Фамилия клиента может содержать только буквы.");
        
        RuleFor(c => c.NumPassport)
            .NotEmpty()
            .NotNull()
            .WithMessage("Номер паспорта клиента не может быть пустым.");
        
        RuleFor(c => c.DateBirthday)
            .NotEmpty()
            .WithMessage("Дата рождения клиента не может быть пустой.")
            .Must(ValidateAge)
            .WithMessage("Клиент должен быть старше 18 лет.")
            .Must(ValidateDateBirthday)
            .WithMessage("Дата рождения клиента должна быть меньше, чем нынешняя.");

        RuleFor(c => c.Phone)
            .NotNull()
            .NotEmpty()
            .WithMessage("Номер телефона клиента не может быть пустым");
    }
    private bool ValidateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;
        return age >= 18;
    }

    private bool ValidateDateBirthday(DateTime date)
    {
        return date <= DateTime.Today;
    }
}