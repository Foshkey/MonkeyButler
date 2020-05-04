using System;
using System.Collections.Generic;
using MonkeyButler.Data.Models.XivApi.Character;

namespace MonkeyButler.Business.Engines
{
    internal class CharacterNameQueryEngine : ICharacterNameQueryEngine
    {
        private readonly List<string> _serverNames = new List<string>()
        {
            "Aegis", "Atomos", "Carbuncle", "Garuda", "Gungnir", "Kujata", "Ramuh", "Tonberry", "Typhon", "Unicorn", "Gaia", "Alexander", "Bahamut", "Durandal", "Fenrir", "Ifrit", "Ridill", "Tiamat", "Ultima", "Valefor", "Yojimbo", "Zeromus", "Mana", "Anima", "Asura", "Belias", "Chocobo", "Hades", "Ixion", "Mandragora", "Masamune", "Pandaemonium", "Shinryu", "Titan",
            "Adamantoise", "Cactuar", "Faerie", "Gilgamesh", "Jenova", "Midgardsormr", "Sargatanas", "Siren", "Primal", "Behemoth", "Excalibur", "Exodus", "Famfrit", "Hyperion", "Lamia", "Leviathan", "Ultros", "Crystal", "Balmung", "Brynhildr", "Coeurl", "Diabolos", "Goblin", "Malboro", "Mateus", "Zalera",
            "Cerberus", "Louisoix", "Moogle", "Omega", "Ragnarok", "Spriggan", "Light", "Lich", "Odin", "Phoenix", "Shiva", "Twintania", "Zodiark"
        };

        public SearchQuery Parse(string input)
        {
            var split = input.Split(' ');
            var query = new SearchQuery();

            if (split.Length == 1)
            {
                // Assume just the name
                query.Name = input;
                return query;
            }

            // First word
            var server = _serverNames.Find(x => string.Equals(x, split[0], StringComparison.OrdinalIgnoreCase));

            if (server is object)
            {
                // First word is server, rest is the name.
                query.Name = string.Join(" ", split[1..]);
                query.Server = server;
                return query;
            }

            // Second word
            server = _serverNames.Find(x => string.Equals(x, split[1], StringComparison.OrdinalIgnoreCase));

            if (server is object)
            {
                // At least the first word is the name.
                query.Name = split[0];

                if (split.Length > 2)
                {
                    // Might as well add in the rest
                    query.Name += " " + string.Join(" ", split[2..]);
                }

                query.Server = server;
                return query;
            }

            // At this point just assume the very last word is the server.
            query.Name = string.Join(" ", split[..^1]);
            query.Server = split[^1];
            return query;
        }
    }

    internal interface ICharacterNameQueryEngine
    {
        SearchQuery Parse(string input);
    }
}
