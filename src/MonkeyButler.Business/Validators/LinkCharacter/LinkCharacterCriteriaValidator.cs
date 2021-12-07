using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.LinkCharacter;

namespace MonkeyButler.Business.Validators.LinkCharacter;

internal class LinkCharacterCriteriaValidator : AbstractValidator<LinkCharacterCriteria>
{
    public LinkCharacterCriteriaValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan((ulong)0);

        RuleFor(x => x.Query)
            .NotEmpty();
    }
}
