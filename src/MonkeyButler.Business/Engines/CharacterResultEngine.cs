using System.Linq;
using MonkeyButler.Business.Models.CharacterSearch;
using MonkeyButler.Data.Models.XivApi.Character;
using MonkeyButler.Data.Models.XivApi.Enumerations;

namespace MonkeyButler.Business.Engines
{
    internal class CharacterResultEngine : ICharacterResultEngine
    {
        public Character Merge(CharacterBrief character, GetCharacterData details)
        {
            return new Character()
            {
                AvatarUrl = character.Avatar,
                CurrentClassJob = details.Character?.ActiveClassJob is object ? new Models.CharacterSearch.ClassJob()
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

        private string? Capitalize(string? str)
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
        private string? RemoveDuplicates(string? str)
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

        private string? ConvertRace(Race? race)
        {
            return race switch
            {
                Race.AuRa => "Au Ra",
                _ => race?.ToString() ?? null
            };
        }

        private string? ConvertTribe(Tribe? tribe)
        {
            return tribe switch
            {
                Tribe.SeaWolf => "Sea Wolf",
                Tribe.SeekerOfTheMoon => "Seeker of the Moon",
                Tribe.SeekerOfTheSun => "Seeker of the Sun",
                _ => tribe?.ToString() ?? null
            };
        }
    }

    internal interface ICharacterResultEngine
    {
        Character Merge(CharacterBrief character, GetCharacterData details);
    }
}
