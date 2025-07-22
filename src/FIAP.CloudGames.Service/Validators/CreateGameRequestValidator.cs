using FIAP.CloudGames.Domain.Requests.Game;
using FluentValidation;

namespace FIAP.CloudGames.Service.Validators;

public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
{
    public CreateGameRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(2).WithMessage("Title must have at least 2 characters.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(10).WithMessage("Description must have at least 10 characters.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or positive.")
            .LessThanOrEqualTo(9999.99m).WithMessage("Price cannot exceed 9999.99.");

        RuleFor(x => x.Genre)
            .IsInEnum().WithMessage("Invalid game genre.");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Release date cannot be in the future.");
    }
}