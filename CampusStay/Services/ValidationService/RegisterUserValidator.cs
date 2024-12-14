using CampusStay.DTO.Users;
using FluentValidation;

namespace CampusStay.Services.ValidationService
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {        


        public RegisterUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 character.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 character long");

            RuleFor(user => user.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches("^(\\+998|998)?(33|55|77|88|90|91|93|94|95|97|98|99)\\d{7}$").WithMessage("Write phone number as +998XXXXXXXXX");
        }
    }
}
