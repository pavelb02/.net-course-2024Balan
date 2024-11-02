using BankSystem.App.Dto;
using FluentValidation;

namespace BankSystem.App.Validators;

public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Имя сотрудника не может быть пустым.")
            .Length(2, 20)
            .WithMessage("Имя сотрудника должно содержать от 2 до 20 символов.")
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Имя сотрудника может содержать только буквы.");
        
        RuleFor(c => c.Surname)
            .NotEmpty()
            .WithMessage("Фамилия сотрудника не может быть пустой.")
            .Length(2, 20)
            .WithMessage("Фамилия сотрудника должна содержать от 2 до 20 символов.")
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Фамилия сотрудника может содержать только буквы.");
        
        RuleFor(c => c.NumPassport)
            .NotEmpty()
            .NotNull()
            .WithMessage("Номер паспорта сотрудника не может быть пустым.");
        
        RuleFor(c => c.DateBirthday)
            .NotEmpty()
            .NotNull()
            .WithMessage("Дата рождения сотрудника не может быть пустой.");

        RuleFor(c => c.Phone)
            .NotNull()
            .NotEmpty()
            .WithMessage("Номер телефона сотрудника не может быть пустым");
    }
}