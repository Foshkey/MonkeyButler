using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;

namespace MonkeyButler.Business.Validators.Options;

internal class SetPrefixValidator : AbstractValidator<SetPrefixCriteria>
{
    public SetPrefixValidator()
    {
        RuleFor(x => x.GuildId)
            .GreaterThan((ulong)0);

        RuleFor(x => x.Prefix)
            .NotEmpty();
    }
}
