using System.Collections.Concurrent;
using System.Threading.Tasks;
using MonkeyButler.Models.Character;

namespace MonkeyButler.Extensions
{
    internal static class CharacterExtensions
    {
        public static async Task<CharacterSearchResponse> ToDisplayModel(this Business.Models.CharacterSearch.CharacterSearchResult result)
        {
            var displayCharacters = new ConcurrentBag<Character>();

            if (result.Characters is null)
            {
                return new CharacterSearchResponse()
                {
                    Characters = displayCharacters
                };
            }

            await foreach (var character in result.Characters)
            {
                displayCharacters.Add(new Character()
                {
                    AvatarUrl = character.AvatarUrl,
                    CurrentClassJob = character.CurrentClassJob is object ? new ClassJob()
                    {
                        Level = character.CurrentClassJob.Level,
                        Name = character.CurrentClassJob.Name
                    } : null,
                    FreeCompany = character.FreeCompany,
                    Id = character.Id,
                    LodestoneUrl = character.LodestoneUrl,
                    Name = character.Name,
                    Race = character.Race,
                    Server = character.Server,
                    Tribe = character.Tribe
                });
            }

            return new CharacterSearchResponse()
            {
                Characters = displayCharacters
            };
        }
    }
}
