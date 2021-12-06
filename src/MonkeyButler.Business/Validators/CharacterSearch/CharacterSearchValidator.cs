using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.CharacterSearch;

namespace MonkeyButler.Business.Validators.CharacterSearch;

internal class CharacterSearchValidator : AbstractValidator<CharacterSearchCriteria>
{
    public CharacterSearchValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty();
    }
}
