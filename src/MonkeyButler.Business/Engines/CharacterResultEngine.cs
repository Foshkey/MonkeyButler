using System.Linq;
using MonkeyButler.Abstractions.Business.Models.CharacterSearch;
using MonkeyButler.Abstractions.Data.Api.Models.Character;
using MonkeyButler.Abstractions.Data.Api.Models.Enumerations;

namespace MonkeyButler.Business.Engines
{
    internal static class CharacterResultEngine
    {
        public static Character Merge(CharacterBrief character, GetCharacterData details)
        {
            return new Character()
            {
                AvatarUrl = character.Avatar,
                CurrentClassJob = details.Character?.ActiveClassJob is object ? new Abstractions.Business.Models.CharacterSearch.ClassJob()
                {
                    Level = details.Character?.ActiveClassJob?.Level ?? 0,
                    Name = RemoveDuplicates(Capitalize(details.Character?.ActiveClassJob?.Name))
                } : null,
                FreeCompany = details.FreeCompany?.Name,
                Id = character.Id,
                LodestoneUrl = $"https://na.finalfantasyxiv.com/lodestone/character/{character.Id}",
                Name = character.Name,
                Race = ConvertRace(details.Character?.Race),
                Server = character.Server,
                Tribe = ConvertTribe(details.Character?.Tribe)
            };
        }

        private static string? Capitalize(string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var capitalizedSplit = str.Split(' ')
                .Select(x => x.Length > 0
                    ? char.ToUpper(x[0]) + x.Substring(1).ToLower()
                    : x);

            return string.Join(" ", capitalizedSplit);
        }

        // For class-less jobs, XivApi returns e.g. "dark knight / dark knight"
        private static string? RemoveDuplicates(string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var split = str.Split('/')
                .Select(x => x.Trim())
                .ToArray();

            if (split.Length >= 2 && split[0] == split[1])
            {
                return split[0];
            }

            return str;
        }

        private static string? ConvertRace(Race? race)
        {
            return race switch
            {
                Race.AuRa => "Au Ra",
                _ => race?.ToString() ?? null
            };
        }

        private static string? ConvertTribe(Tribe? tribe)
        {
            return tribe switch
            {
                Tribe.SeaWolf => "Sea Wolf",
                Tribe.SeekerOfTheMoon => "Seeker of the Moon",
                Tribe.SeekerOfTheSun => "Seeker of the Sun",
                Tribe.TheLost => "The Lost",
                _ => tribe?.ToString() ?? null
            };
        }
    }
}
