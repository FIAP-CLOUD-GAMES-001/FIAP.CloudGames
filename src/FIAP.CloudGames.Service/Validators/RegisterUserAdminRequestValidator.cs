using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.Domain.Requests.User;
using FluentValidation;

namespace FIAP.CloudGames.Service.Validators;

public class RegisterUserAdminRequestValidator : AbstractValidator<RegisterUserAdminRequest>
{
    public RegisterUserAdminRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must have at least 2 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[!@#$%^&*()]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.Role)
            .Must(r => Enum.IsDefined(typeof(ERole), r))
            .WithMessage("Invalid role value.");
    }
}