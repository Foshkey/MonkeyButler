﻿namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character;

/// <summary>
/// The query for the get command.
/// </summary>
public record GetCharacterQuery
{
    /// <summary>
    /// The id of the character.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The extra data request for the character.
    /// </summary>
    public string? Data { get; set; }
}
