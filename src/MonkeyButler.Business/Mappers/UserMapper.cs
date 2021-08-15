using BusinessUser = MonkeyButler.Abstractions.Business.Models.User.User;
using DataUser = MonkeyButler.Abstractions.Data.Storage.Models.User.User;

namespace MonkeyButler.Business.Mappers
{
    internal static class UserMapper
    {
        public static BusinessUser? ToBusiness(this DataUser? user) =>
            user is null ? null : new()
            {
                Id = user.Id,
                CharacterIds = user.CharacterIds,
                Name = user.Name,
                Nicknames = user.Nicknames
            };

        public static DataUser? ToData(this BusinessUser? user) =>
            user is null ? null : new()
            {
                Id = user.Id,
                CharacterIds = new(user.CharacterIds),
                Name = user.Name,
                Nicknames = new(user.Nicknames)
            };
    }
}
