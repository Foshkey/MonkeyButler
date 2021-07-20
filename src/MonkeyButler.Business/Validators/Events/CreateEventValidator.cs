using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Events;

namespace MonkeyButler.Business.Validators.Events
{
    internal class CreateEventValidator : AbstractValidator<CreateEventCriteria>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.GuildId)
                .GreaterThan((ulong)0);

            RuleFor(x => x.Query)
                .NotEmpty();

            RuleFor(x => x.VoiceRegionId)
                .NotEmpty();
        }
    }
}
