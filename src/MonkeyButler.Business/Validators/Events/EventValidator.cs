using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Events;

namespace MonkeyButler.Business.Validators.Events
{
    internal class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((ulong)0);
        }
    }
}