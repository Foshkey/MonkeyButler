﻿namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.Info;

/// <summary>
/// Info of the response.
/// </summary>
public record Info
{
    /// <summary>
    /// The info on achievements.
    /// </summary>
    public InfoSegment? Achievements { get; set; }

    /// <summary>
    /// The info on character.
    /// </summary>
    public InfoSegment? Character { get; set; }

    /// <summary>
    /// The info on free company.
    /// </summary>
    public InfoSegment? FreeCompany { get; set; }

    /// <summary>
    /// The info on free company members.
    /// </summary>
    public InfoSegment? FreeCompanyMembers { get; set; }

    /// <summary>
    /// The info on friends.
    /// </summary>
    public InfoSegment? Friends { get; set; }

    /// <summary>
    /// The info on pvp team.
    /// </summary>
    public InfoSegment? PvpTeam { get; set; }
}
