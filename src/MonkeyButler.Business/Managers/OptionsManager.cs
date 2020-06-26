using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Models.Options;
using MonkeyButler.Data.Cache;
using MonkeyButler.Data.Database.Guild;
using MonkeyButler.Data.XivApi.FreeCompany;

namespace MonkeyButler.Business.Managers
{
    internal class OptionsManager : IOptionsManager
    {
        private readonly ICacheAccessor _cacheAccessor;
        private readonly IFreeCompanyAccessor _freeCompanyAccessor;
        private readonly IGuildAccessor _guildAccessor;
        private readonly ILogger<OptionsManager> _logger;
        private readonly IValidator<SetSignupEmotesCriteria> _setSignupEmotesValidator;
        private readonly IValidator<SetVerificationCriteria> _setVerificationValidator;

        public OptionsManager(
            ICacheAccessor cacheAccessor,
            IFreeCompanyAccessor freeCompanyAccessor,
            IGuildAccessor guildAccessor,
            ILogger<OptionsManager> logger,
            IValidator<SetSignupEmotesCriteria> setSignupEmotesValidator,
            IValidator<SetVerificationCriteria> setVerificationValidator)
        {
            _cacheAccessor = cacheAccessor ?? throw new ArgumentNullException(nameof(cacheAccessor));
            _freeCompanyAccessor = freeCompanyAccessor ?? throw new ArgumentNullException(nameof(freeCompanyAccessor));
            _guildAccessor = guildAccessor ?? throw new ArgumentNullException(nameof(guildAccessor));
            _logger = logger;
            _setSignupEmotesValidator = setSignupEmotesValidator ?? throw new ArgumentNullException(nameof(setSignupEmotesValidator));
            _setVerificationValidator = setVerificationValidator ?? throw new ArgumentNullException(nameof(setVerificationValidator));
        }

        public Task SetSignupEmotes(SetSignupEmotesCriteria criteria)
        {
            _setSignupEmotesValidator.Validate(criteria);

            throw new NotImplementedException();
        }

        public Task<SetVerificationResult> SetVerification(SetVerificationCriteria criteria)
        {
            _setVerificationValidator.Validate(criteria);

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Manager for managing options.
    /// </summary>
    public interface IOptionsManager
    {
        /// <summary>
        /// Sets sign-up emotes of the guild.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        Task SetSignupEmotes(SetSignupEmotesCriteria criteria);

        /// <summary>
        /// Sets verification options of the guild.
        /// </summary>
        /// <param name="criteria">The criteria for setting verification options.</param>
        /// <returns>The result of the request.</returns>
        Task<SetVerificationResult> SetVerification(SetVerificationCriteria criteria);
    }
}
