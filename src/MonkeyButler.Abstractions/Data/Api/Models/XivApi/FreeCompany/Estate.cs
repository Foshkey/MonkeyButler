﻿namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.FreeCompany;

/// <summary>
/// Model representing a Free Company Estate.
/// </summary>
public record Estate
{
    /// <summary>
    /// The greeting of the estate.
    /// </summary>
    public string? Greeting { get; set; }

    /// <summary>
    /// The name of the estate.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The plot that the estate occupies.
    /// </summary>
    public string? Plot { get; set; }
}
