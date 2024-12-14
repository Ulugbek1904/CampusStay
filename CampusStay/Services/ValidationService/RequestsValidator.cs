using CampusStay.DTO.Requests;
using FluentValidation;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace CampusStay.Services.ValidationService
{
    public class RequestsValidator : AbstractValidator<ResetPasswordDto>
    {
        public RequestsValidator()
        {
            RuleFor(request => request.NewPassword)
                .NotEmpty().WithMessage("New Password is required")
                .MinimumLength(7).WithMessage("At least 7 character");

            RuleFor(request => request.ConfirmPassword)
                .NotEmpty().WithMessage("New Password is required")
                .MinimumLength(7).WithMessage("At least 7 character");

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }
    }
}
