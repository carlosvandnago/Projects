using FluentValidation;

namespace TaxRacm.Clients.Application.Commands.CreateEntity;

public class CreateEntityCommandValidator : AbstractValidator<CreateEntityCommand>
{
    public CreateEntityCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Country).NotEmpty().Length(2);
        RuleFor(x => x.Jurisdiction).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EntityType).NotEmpty();
        RuleFor(x => x.Region).NotEmpty();
    }
}
