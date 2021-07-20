using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;

namespace MonkeyButler.Business.Validators.Options
{
    internal class GetGuildOptionsValicator : AbstractValidator<GuildOptionsCriteria>
    {
        public GetGuildOptionsValicator()
        {
            RuleFor(x => x.GuildId)
                .GreaterThan((ulong)0);
        }
    }
}
