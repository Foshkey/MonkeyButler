using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Events;

namespace MonkeyButler.Business.Validators.Events
{
    internal class DeleteEventValidator : AbstractValidator<DeleteEventCriteria>
    {
        public DeleteEventValidator()
        {
            RuleFor(x => x.EventId).GreaterThan(0);
        }
    }
}
