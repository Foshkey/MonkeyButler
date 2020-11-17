using System;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Data.Models.Database.User;
using MonkeyButler.Data.Options;

namespace MonkeyButler.Data.Database
{
    internal class UserAccessor : IUserAccessor
    {
        private readonly IOptionsMonitor<LiteDbOptions> _liteDbOptions;
        private readonly ILogger<UserAccessor> _logger;

        private readonly string _userKey = "Users";

        public UserAccessor(IOptionsMonitor<LiteDbOptions> liteDbOptions, ILogger<UserAccessor> logger)
        {
            _liteDbOptions = liteDbOptions ?? throw new ArgumentNullException(nameof(liteDbOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<User?> GetVerifiedUser(GetVerifiedUserQuery query)
        {
            _logger.LogDebug("Getting user based on character {CharacterId}.", query.CharacterId);

            using var db = new LiteDatabase(_liteDbOptions.CurrentValue.File);
            var users = db.GetCollection<User>(_userKey);

            var user = await Task.Run(() => users.FindOne(x => x.CharacterIds.Contains(query.CharacterId)));

            return user;
        }

        public async Task SaveCharacterToUser(SaveCharacterToUserQuery query)
        {
            _logger.LogDebug("Saving character {CharacterId} to User {UserId}.", query.CharacterId, query.UserId);

            using var db = new LiteDatabase(_liteDbOptions.CurrentValue.File);
            var users = db.GetCollection<User>(_userKey);
            users.EnsureIndex(x => x.Id);
            users.EnsureIndex(x => x.CharacterIds);

            // Find or create new
            var user = await Task.Run(() => users.FindOne(x => x.Id == query.UserId))
                ?? new User()
                {
                    Id = query.UserId
                };

            // Add to list
            user.CharacterIds.Add(query.CharacterId);

            // Update
            await Task.Run(() => users.Upsert(user));
        }
    }

    /// <summary>
    /// Database user accessor
    /// </summary>
    public interface IUserAccessor
    {
        /// <summary>
        /// Gets the user for a given character Id. Null if not found.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<User?> GetVerifiedUser(GetVerifiedUserQuery query);

        /// <summary>
        /// Saves the options for a guild.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task SaveCharacterToUser(SaveCharacterToUserQuery query);
    }
}
