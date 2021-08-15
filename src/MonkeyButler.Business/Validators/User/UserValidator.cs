using FluentValidation;
using BusinessUser = MonkeyButler.Abstractions.Business.Models.User.User;

namespace MonkeyButler.Business.Validators.User
{
    internal class UserValidator : AbstractValidator<BusinessUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan((ulong)0);

            RuleFor(x => x.CharacterIds)
                .NotNull();
        }
    }
}
