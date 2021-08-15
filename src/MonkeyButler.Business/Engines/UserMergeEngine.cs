using System.Collections.Generic;
using System.Linq;
using MonkeyButler.Abstractions.Data.Storage.Models.User;

namespace MonkeyButler.Business.Engines
{
    internal static class UserMergeEngine
    {
        public static User Merge(this User user, IEnumerable<long>? characterIds)
        {
            if (characterIds is null || !characterIds.Any())
            {
                return user;
            }

            var id = user.Id;
            var originalCharacterIds = new HashSet<long>(user.CharacterIds);
            originalCharacterIds.UnionWith(characterIds);

            return new()
            {
                Id = id,
                CharacterIds = originalCharacterIds
            };
        }

        public static User Merge(this User user1, User? user2) => user1.Merge(user2?.CharacterIds);

        public static User Merge(this User user, long characterId) => user.Merge(new HashSet<long>() { characterId });
    }
}
