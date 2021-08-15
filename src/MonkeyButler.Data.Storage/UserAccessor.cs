using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.User;

namespace MonkeyButler.Data.Storage
{
    internal class UserAccessor : IUserAccessor
    {
        private readonly IDistributedCache _distributedCache;

        private readonly string _usersKey = "MonkeyButler:Users";
        private readonly string _charactersKey = "MonkeyButler:Characters";

        public UserAccessor(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        private string GetLinkKey(long characterId) => $"{_charactersKey}:{characterId}:User";

        public async Task<User?> GetUser(ulong id)
        {
            var key = $"{_usersKey}:{id}";
            var user = await _distributedCache.GetStringAsync(key);

            if (string.IsNullOrEmpty(user))
            {
                return null;
            }

            return JsonSerializer.Deserialize<User>(user);
        }

        public async Task<User?> SearchUser(SearchUserQuery query)
        {
            var characterKey = GetLinkKey(query.CharacterId);
            var userIdString = await _distributedCache.GetStringAsync(characterKey);

            if (!ulong.TryParse(userIdString, out var userId))
            {
                return null;
            }

            return await GetUser(userId);
        }

        public async Task<User> SaveUser(User user)
        {
            // Update
            var key = $"{_usersKey}:{user.Id}";
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(user));

            // Add search links
            await Task.WhenAll(user.CharacterIds.Select(charId =>
                _distributedCache.SetStringAsync(GetLinkKey(charId), user.Id.ToString())));

            return await GetUser(user.Id) ?? throw new InvalidOperationException("Could not save user to data store.");
        }
    }
}
