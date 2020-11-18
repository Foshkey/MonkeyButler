using FluentValidation;
using MonkeyButler.Business.Models.VerifyCharacter;

namespace MonkeyButler.Business.Validators.VerifyCharacter
{
    internal class VerifyCharacterValidator : AbstractValidator<VerifyCharacterCriteria>
    {
        public VerifyCharacterValidator()
        {
            RuleFor(x => x.GuildId)
                .GreaterThan((ulong)0);

            RuleFor(x => x.UserId)
                .GreaterThan((ulong)0);

            RuleFor(x => x.Query)
                .NotEmpty();
        }
    }
}
