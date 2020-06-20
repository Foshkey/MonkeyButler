using FluentValidation;
using MonkeyButler.Business.Models.Events;

namespace MonkeyButler.Business.Validators.Events
{
    internal class CreateEventValidator : AbstractValidator<CreateEventCriteria>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Query).NotEmpty();
        }
    }
}
