using FluentValidation;
using MonkeyButler.Models.Character;

namespace MonkeyButler.Validators.Character
{
    /// <summary>
    /// Validator for <see cref="CharacterSearchRequest"/>.
    /// </summary>
    public class CharacterSearchRequestValidator : AbstractValidator<CharacterSearchRequest>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public CharacterSearchRequestValidator()
        {
            RuleFor(x => x.Query)
                .NotEmpty();
        }
    }
}
