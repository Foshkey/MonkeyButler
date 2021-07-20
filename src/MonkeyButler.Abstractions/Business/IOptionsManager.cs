using System.Threading.Tasks;
using MonkeyButler.Abstractions.Business.Models.Options;

namespace MonkeyButler.Abstractions.Business
{
    /// <summary>
    /// Manager for managing options.
    /// </summary>
    public interface IOptionsManager
    {
        /// <summary>
        /// Gets the guild options from data store.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<GuildOptionsResult?> GetGuildOptions(GuildOptionsCriteria criteria);

        /// <summary>
        /// Sets the prefix for the guild.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<SetPrefixResult> SetPrefix(SetPrefixCriteria criteria);

        /// <summary>
        /// Sets sign-up emotes of the guild.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        Task<SetSignupEmotesResult> SetSignupEmotes(SetSignupEmotesCriteria criteria);

        /// <summary>
        /// Sets verification options of the guild.
        /// </summary>
        /// <param name="criteria">The criteria for setting verification options.</param>
        /// <returns>The result of the request.</returns>
        Task<SetVerificationResult> SetVerification(SetVerificationCriteria criteria);
    }
}
