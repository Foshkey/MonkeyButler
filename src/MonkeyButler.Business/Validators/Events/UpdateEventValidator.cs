using FluentValidation;
using MonkeyButler.Business.Models.Events;

namespace MonkeyButler.Business.Validators.Events
{
    internal class UpdateEventValidator : AbstractValidator<UpdateEventCriteria>
    {
        public UpdateEventValidator()
        {
            RuleFor(x => x.Event)
                .NotNull()
                .SetValidator(new EventValidator());
        }
    }
}
