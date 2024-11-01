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
            .WithMessage("Имя должно содержать от 2 до 20 символов.")
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Имя может содержать только буквы.");
        
        RuleFor(c => c.Surname)
            .NotEmpty()
            .WithMessage("Фамилия клиента не может быть пустой.")
            .Length(2, 20)
            .WithMessage("Фамилия должна содержать от 2 до 20 символов.")
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Фамилия может содержать только буквы.");
        
        RuleFor(c => c.NumPassport)
            .NotEmpty()
            .NotNull()
            .WithMessage("Номер паспорта не может быть пустым.");
        
        RuleFor(c => c.DateBirthday)
            .NotEmpty()
            .NotNull()
            .WithMessage("Возраст не может быть пустым.");

        RuleFor(c => c.Phone)
            .NotNull()
            .NotEmpty()
            .WithMessage("Номер телефона не может быть пустым");
    }
}