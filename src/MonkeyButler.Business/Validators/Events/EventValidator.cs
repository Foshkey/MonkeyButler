using FluentValidation;
using MonkeyButler.Business.Models.Events;

namespace MonkeyButler.Business.Validators.Events
{
    internal class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}