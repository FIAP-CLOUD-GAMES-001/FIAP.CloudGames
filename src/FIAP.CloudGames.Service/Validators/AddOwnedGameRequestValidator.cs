using FIAP.CloudGames.Domain.Requests.Game;
using FluentValidation;

namespace FIAP.CloudGames.Service.Validators;

public class AddOwnedGameRequestValidator : AbstractValidator<AddOwnedGameRequest>
{
    public AddOwnedGameRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be a positive number.");

        RuleFor(x => x.GameId)
            .GreaterThan(0).WithMessage("Game ID must be a positive number.");
    }
}