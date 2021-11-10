using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;

namespace MonkeyButler.Business.Validators.Options;

internal class GetGuildOptionsValidator : AbstractValidator<GuildOptionsCriteria>
{
    public GetGuildOptionsValidator()
    {
        RuleFor(x => x.GuildId)
            .GreaterThan((ulong)0);
    }
}
