﻿using MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character;
using MonkeyButler.Abstractions.Data.Api.Models.XivApi.Enumerations;
using MonkeyButler.Business.Engines;
using Xunit;

namespace MonkeyButler.Business.Tests.Engines;

public class CharacterResultEngineTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("h", "H")]
    [InlineData("hElLo", "Hello")]
    [InlineData("gladiator / paladin", "Gladiator / Paladin")]
    [InlineData("double  space", "Double  Space")]
    [InlineData("so       many          spaces", "So       Many          Spaces")]
    public void ShouldCapitilizeClassJobName(string? input, string? expectedName)
    {
        var characterBrief = new CharacterBrief();
        var details = new GetCharacterData()
        {
            Character = new CharacterFull()
            {
                ActiveClassJob = new ClassJob()
                {
                    Name = input
                }
            }
        };

        var result = CharacterResultEngine.Merge(characterBrief, details);

        Assert.Equal(expectedName, result.CurrentClassJob?.Name);
    }

    [Theory]
    [InlineData("Dark Knight", "Dark Knight")]
    [InlineData("Gladiator / Paladin", "Gladiator / Paladin")]
    [InlineData("Dark Knight / Dark Knight", "Dark Knight")]
    [InlineData("dark knight / dark knight", "Dark Knight")]
    [InlineData("dark knight / Dark Knight", "Dark Knight")]
    public void ShouldRemoveDuplicates(string? input, string? expectedName)
    {
        var characterBrief = new CharacterBrief();
        var details = new GetCharacterData()
        {
            Character = new CharacterFull()
            {
                ActiveClassJob = new ClassJob()
                {
                    Name = input
                }
            }
        };

        var result = CharacterResultEngine.Merge(characterBrief, details);

        Assert.Equal(expectedName, result.CurrentClassJob?.Name);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(Race.Unknown, "Unknown")]
    [InlineData(Race.Hyur, "Hyur")]
    [InlineData(Race.AuRa, "Au Ra")]
    public void ShouldConvertRace(Race? race, string? expectedRace)
    {
        var characterBrief = new CharacterBrief();
        var details = new GetCharacterData()
        {
            Character = new CharacterFull()
            {
                Race = race
            }
        };

        var result = CharacterResultEngine.Merge(characterBrief, details);

        Assert.Equal(expectedRace, result.Race);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(Tribe.Unknown, "Unknown")]
    [InlineData(Tribe.Hellsguard, "Hellsguard")]
    [InlineData(Tribe.SeaWolf, "Sea Wolf")]
    [InlineData(Tribe.SeekerOfTheMoon, "Seeker of the Moon")]
    [InlineData(Tribe.SeekerOfTheSun, "Seeker of the Sun")]
    public void ShouldConvertTribe(Tribe? tribe, string? expectedTribe)
    {
        var characterBrief = new CharacterBrief();
        var details = new GetCharacterData()
        {
            Character = new CharacterFull()
            {
                Tribe = tribe
            }
        };

        var result = CharacterResultEngine.Merge(characterBrief, details);

        Assert.Equal(expectedTribe, result.Tribe);
    }

    [Theory]
    [InlineData(0, "https://na.finalfantasyxiv.com/lodestone/character/0")]
    [InlineData(123456, "https://na.finalfantasyxiv.com/lodestone/character/123456")]
    public void ShouldGenerateLodestoneUrl(long id, string expectedUrl)
    {
        var characterBrief = new CharacterBrief()
        {
            Id = id
        };
        var details = new GetCharacterData();

        var result = CharacterResultEngine.Merge(characterBrief, details);

        Assert.Equal(expectedUrl, result.LodestoneUrl);
    }
}
