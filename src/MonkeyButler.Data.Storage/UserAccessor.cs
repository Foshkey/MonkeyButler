using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.User;

namespace MonkeyButler.Data.Storage
{
    internal class UserAccessor : IUserAccessor
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<UserAccessor> _logger;

        private readonly string _usersKey = "MonkeyButler:Users";
        private readonly string _charactersKey = "MonkeyButler:Characters";

        public UserAccessor(IDistributedCache distributedCache, ILogger<UserAccessor> logger)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<User?> GetUser(GetUserQuery query)
        {
            _logger.LogDebug("Getting user '{UserId}'.", query.UserId);

            var key = $"{_usersKey}:{query.UserId}";
            var user = await _distributedCache.GetStringAsync(key);

            if (string.IsNullOrEmpty(user))
            {
                _logger.LogTrace("User not found. Returning null.");
                return null;
            }

            return JsonSerializer.Deserialize<User>(user);
        }

        public async Task<User?> SearchUser(SearchUserQuery query)
        {
            _logger.LogDebug("Searching for user based on character '{CharacterId}'.", query.CharacterId);

            var characterKey = $"{_charactersKey}:{query.CharacterId}:User";
            var userIdString = await _distributedCache.GetStringAsync(characterKey);

            if (!ulong.TryParse(userIdString, out var userId))
            {
                _logger.LogTrace("Character user map not found. Returning null.");
                return null;
            }

            var user = await GetUser(new GetUserQuery()
            {
                UserId = userId
            });

            return user;
        }

        public async Task<User> SaveCharacterToUser(SaveCharacterToUserQuery query)
        {
            _logger.LogDebug("Saving character '{CharacterId}' to User '{UserId}'.", query.CharacterId, query.UserId);


            // Find or create new
            var user = await GetUser(new GetUserQuery() { UserId = query.UserId })
                ?? new User()
                {
                    Id = query.UserId
                };

            // Add to list
            user.CharacterIds.Add(query.CharacterId);

            var tasks = new List<Task>();

            // Update
            var key = $"{_usersKey}:{query.UserId}";
            tasks.Add(_distributedCache.SetStringAsync(key, JsonSerializer.Serialize(user)));

            // Add link to user
            var linkKey = $"{_charactersKey}:{query.CharacterId}:User";
            tasks.Add(_distributedCache.SetStringAsync(linkKey, query.UserId.ToString()));

            await Task.WhenAll(tasks);

            return user;
        }
    }
}
