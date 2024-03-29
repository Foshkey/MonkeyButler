﻿namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.FreeCompany;

/// <summary>
/// Query for the search command.
/// </summary>
public record SearchFreeCompanyQuery
{
    /// <summary>
    /// The name of the character.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The server of the character.
    /// </summary>
    public string? Server { get; set; }
}
