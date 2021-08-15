using System.Collections.Generic;
using System.Linq;
using MonkeyButler.Abstractions.Data.Storage.Models.User;

namespace MonkeyButler.Business.Engines
{
    internal static class UserMergeEngine
    {
        public static User Merge(this User user1, User? user2)
        {
            if (user1 is null)
            {
                return user2 ?? new User();
            }

            if (user2 is null)
            {
                return user1;
            }

            var charIds = new HashSet<long>(user1.CharacterIds);
            charIds.UnionWith(user2.CharacterIds);

            var name = !string.IsNullOrEmpty(user1.Name) ? user1.Name : user2.Name;

            var nicknames = new Dictionary<ulong, string>(user1.Nicknames);
            user2.Nicknames.ToList().ForEach(nickname => nicknames.TryAdd(nickname.Key, nickname.Value));

            return new()
            {
                Id = user1.Id,
                CharacterIds = charIds,
                Name = name,
                Nicknames = nicknames
            };
        }

        public static User Merge(this User user, IEnumerable<long> characterIds) =>
            user.Merge(new User() { CharacterIds = new HashSet<long>(characterIds) });

        public static User Merge(this User user, long characterId) =>
            user.Merge(new HashSet<long>() { characterId });
    }
}
