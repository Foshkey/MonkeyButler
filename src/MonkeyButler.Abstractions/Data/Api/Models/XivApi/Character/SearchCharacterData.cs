﻿namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character;

/// <summary>
/// Data result of character search.
/// </summary>
public record SearchCharacterData
{
    /// <summary>
    /// The pagination of the response.
    /// </summary>
    public Pagination? Pagination { get; set; }

    /// <summary>
    /// The list of characters found, in respects to <see cref="Pagination"/>.
    /// </summary>
    public List<CharacterBrief>? Results { get; set; }
}
