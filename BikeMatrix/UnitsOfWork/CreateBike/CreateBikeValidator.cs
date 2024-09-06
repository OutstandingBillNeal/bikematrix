using FluentValidation;

namespace UnitsOfWork;

public class CreateBikeValidator
    : AbstractValidator<CreateBikeRequest>
{
    public CreateBikeValidator()
    {
        RuleFor(r => r.Bike.Id)
            .Equal(0)
            .WithMessage("must be zero, if supplied");
        RuleFor(r => r.Bike.OwnerEmail)
            .EmailAddress();
        RuleFor(r => r.Bike.Year)
            .InclusiveBetween(1900, DateTime.Now.Year);
    }
}
