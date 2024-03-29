﻿namespace MonkeyButler.Abstractions.Business.Models.CharacterSearch;

/// <summary>
/// The criteria for searching for characters.
/// </summary>
public record CharacterSearchCriteria
{
    /// <summary>
    /// String query for name and server.
    /// </summary>
    public string Query { get; set; } = "";
}
