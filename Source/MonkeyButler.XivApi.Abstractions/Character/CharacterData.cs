using System;
using MonkeyButler.XivApi.Attributes;

namespace MonkeyButler.XivApi.Character
{
    /// <summary>
    /// An enum representing the data parameter of the character service.
    /// </summary>
    [Flags()]
    public enum CharacterData
    {
        /// <summary>
        /// Requests for achievements data.
        /// </summary>
        [ApiName("AC")]
        Achievements = 1,

        /// <summary>
        /// Requests for friends list data.
        /// </summary>
        [ApiName("FR")]
        FriendsList = 2,

        /// <summary>
        /// Requests for free company data.
        /// </summary>
        [ApiName("FC")]
        FreeCompany = 4,

        /// <summary>
        /// Requests for free company members data.
        /// </summary>
        [ApiName("FCM")]
        FreeCompanyMembers = 8,

        /// <summary>
        /// Requests for PvP team data.
        /// </summary>
        [ApiName("PVP")]
        PvpTeam = 16
    }
}
