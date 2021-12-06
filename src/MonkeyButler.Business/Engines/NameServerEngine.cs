namespace MonkeyButler.Business.Engines;

internal static class NameServerEngine
{
    private static readonly List<string> _serverNames = new List<string>()
        {
            "Aegis", "Atomos", "Carbuncle", "Garuda", "Gungnir", "Kujata", "Ramuh", "Tonberry", "Typhon", "Unicorn", "Gaia", "Alexander", "Bahamut", "Durandal", "Fenrir", "Ifrit", "Ridill", "Tiamat", "Ultima", "Valefor", "Yojimbo", "Zeromus", "Mana", "Anima", "Asura", "Belias", "Chocobo", "Hades", "Ixion", "Mandragora", "Masamune", "Pandaemonium", "Shinryu", "Titan",
            "Adamantoise", "Cactuar", "Faerie", "Gilgamesh", "Jenova", "Midgardsormr", "Sargatanas", "Siren", "Primal", "Behemoth", "Excalibur", "Exodus", "Famfrit", "Hyperion", "Lamia", "Leviathan", "Ultros", "Crystal", "Balmung", "Brynhildr", "Coeurl", "Diabolos", "Goblin", "Malboro", "Mateus", "Zalera",
            "Cerberus", "Louisoix", "Moogle", "Omega", "Ragnarok", "Spriggan", "Light", "Lich", "Odin", "Phoenix", "Shiva", "Twintania", "Zodiark"
        };

    public static (string name, string? server) Parse(string input)
    {
        var name = input;
        var split = input.Split(' ');

        if (split.Length == 1)
        {
            // Assume just the name
            return (input, null);
        }

        // First word
        var server = _serverNames.Find(x => string.Equals(x, split[0], StringComparison.OrdinalIgnoreCase));

        if (server is object)
        {
            // First word is server, rest is the name.
            var remaining = string.Join(" ", split[1..]);
            return (remaining, server);
        }

        // Second word
        server = _serverNames.Find(x => string.Equals(x, split[1], StringComparison.OrdinalIgnoreCase));

        if (server is object)
        {
            // At least the first word is the name.
            name = split[0];

            if (split.Length > 2)
            {
                // Might as well add in the rest
                name += " " + string.Join(" ", split[2..]);
            }

            return (name, server);
        }

        if (split.Length == 2)
        {
            // Neither of the first two words are servers, assume full name.
            return (input, null);
        }

        // At this point just assume the very last word is the server.
        name = string.Join(" ", split[..^1]);
        server = split[^1];
        return (name, server);
    }
}
