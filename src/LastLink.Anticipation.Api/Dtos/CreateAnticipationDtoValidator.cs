using FluentValidation;

namespace LastLink.Anticipation.Api.Dtos
{
    public class CreateAnticipationDtoValidator : AbstractValidator<CreateAnticipationDto>
    {
        public CreateAnticipationDtoValidator()
        {
            RuleFor(x => x.CreatorId)
                .NotEmpty().WithMessage("CreatorId is required.");

            RuleFor(x => x.RequestedAmount)
                .GreaterThan(100).WithMessage("Requested amount must be greater than 100.");

            RuleFor(x => x.RequestedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("RequestedAt cannot be in the future.");
        }
    }
}