using FIAP.CloudGames.Domain.Requests.Game;
using FluentValidation;

namespace FIAP.CloudGames.Service.Validators;

public class CreatePromotionRequestValidator : AbstractValidator<CreatePromotionRequest>
{
    public CreatePromotionRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(2).WithMessage("Title must have at least 2 characters.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.DiscountPercentage)
            .GreaterThan(0).WithMessage("Discount percentage must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Discount percentage cannot exceed 100.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");

        RuleFor(x => x.GameId)
            .GreaterThan(0).WithMessage("Game ID must be a positive number.");
    }
}