using FluentValidation;
using MonkeyButler.Business.Models.Events;

namespace MonkeyButler.Business.Validators.Events
{
    internal class CreateEventValidator : AbstractValidator<CreateEventCriteria>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Query)
                .NotEmpty()
                .When(x => x.Event is null);

            RuleFor(x => x.Event)
                .NotEmpty()
                .When(x => string.IsNullOrEmpty(x.Query));
        }
    }
}
