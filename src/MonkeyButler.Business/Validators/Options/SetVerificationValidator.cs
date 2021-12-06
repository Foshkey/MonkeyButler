using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;

namespace MonkeyButler.Business.Validators.Options;

/// <summary>
/// Validator for set verification
/// </summary>
public class SetVerificationValidator : AbstractValidator<SetVerificationCriteria>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public SetVerificationValidator()
    {
        RuleFor(x => x.GuildId)
            .GreaterThan((ulong)0);

        RuleFor(x => x.RoleId)
            .GreaterThan((ulong)0);

        RuleFor(x => x.FreeCompanyAndServer)
            .NotEmpty();
    }
}
