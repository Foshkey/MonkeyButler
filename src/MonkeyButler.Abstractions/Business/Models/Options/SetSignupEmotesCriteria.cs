namespace MonkeyButler.Abstractions.Business.Models.Options;

/// <summary>
/// Criteria for setting sign-up emotes for events.
/// </summary>
public record SetSignupEmotesCriteria
{
    /// <summary>
    /// Id of the guild.
    /// </summary>
    public ulong GuildId { get; set; }

    /// <summary>
    /// The string containing the emotes in raw form, e.g. "&lt;:tank:123456789&gt; &lt;:healer:123456788&gt; &lt;:dps:123456787&gt;"
    /// </summary>
    public string Emotes { get; set; } = null!;
}
