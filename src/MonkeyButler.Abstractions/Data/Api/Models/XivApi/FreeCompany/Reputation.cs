﻿namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.FreeCompany;

/// <summary>
/// A model representing a Free Company reputation with a Grand Company.
/// </summary>
public record Reputation
{
    /// <summary>
    /// The name of the Grand Company associated with this reputation.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The current progress in the reputation.
    /// </summary>
    public int? Progress { get; set; }

    /// <summary>
    /// The current rank in the reputation.
    /// </summary>
    public string? Rank { get; set; }
}
