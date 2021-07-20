using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;

namespace MonkeyButler.Business.Validators.Options
{
    /// <summary>
    /// Validator for setting sign-up emotes.
    /// </summary>
    public class SetSignupEmotesValidator : AbstractValidator<SetSignupEmotesCriteria>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SetSignupEmotesValidator()
        {
            RuleFor(x => x.GuildId)
                .GreaterThan((ulong)0);

            RuleFor(x => x.Emotes)
                .NotEmpty();
        }
    }
}
