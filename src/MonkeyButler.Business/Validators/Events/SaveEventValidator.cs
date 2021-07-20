using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Events;

namespace MonkeyButler.Business.Validators.Events
{
    internal class SaveEventValidator : AbstractValidator<SaveEventCriteria>
    {
        public SaveEventValidator()
        {
            RuleFor(x => x.Event)
                .NotNull()
                .SetValidator(new EventValidator());
        }
    }
}
