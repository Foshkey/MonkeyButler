namespace MonkeyButler.Abstractions.Business.Models.Options;

/// <summary>
/// Criteria for setting verification options.
/// </summary>
public record SetVerificationCriteria
{
    /// <summary>
    /// Id of the guild.
    /// </summary>
    public ulong GuildId { get; set; }

    /// <summary>
    /// The discord role Id to assign.
    /// </summary>
    public ulong RoleId { get; set; }

    /// <summary>
    /// String containing the free company name and FFXIV server name.
    /// </summary>
    public string FreeCompanyAndServer { get; set; } = null!;
}
