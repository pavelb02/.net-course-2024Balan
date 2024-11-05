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
            .WithMessage("Дата рождения сотрудника не может быть пустой.")
            .Must(ValidateAge)
            .WithMessage("Сотрудник должен быть старше 18 лет.")
            .Must(ValidateDateBirthday)
            .WithMessage("Дата рождения сотрудника должна меньше, чем нынешняя.");
        
        RuleFor(e => e.Salary)
            .GreaterThan(0)
            .WithMessage("Зарплата должна быть положительным числом.");
        
        RuleFor(e => e.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Дата начала работы не может быть в будущем.");
        
        RuleFor(c => c.Phone)
            .NotNull()
            .NotEmpty()
            .WithMessage("Номер телефона сотрудника не может быть пустым");
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