using System;
using System.Collections.Generic;

namespace MonkeyButler.Data.Models.XivApi.FreeCompany
{
    /// <summary>
    /// A model representing a FFXIV Free Company.
    /// </summary>
    public class FreeCompany : XivApiModel
    {
        /// <summary>
        /// The current active status of the free company.
        /// </summary>
        public string? Active { get; set; }

        /// <summary>
        /// The number of active members in the free company.
        /// </summary>
        public int? ActiveMemberCount { get; set; }

        /// <summary>
        /// A list of Image URLs that represent the free company crest.
        /// </summary>
        public List<string>? Crest { get; set; }

        /// <summary>
        /// The estate information for the free company.
        /// </summary>
        public Estate? Estate { get; set; }

        /// <summary>
        /// The focuses of the free company.
        /// </summary>
        public List<Attribute>? Focus { get; set; }

        /// <summary>
        /// A DateTime representing when the free company was formed.
        /// </summary>
        public DateTime? Formed { get; set; }

        /// <summary>
        /// The grand company that the free company is allied with.
        /// </summary>
        public string? GrandCompany { get; set; }

        /// <summary>
        /// The Id of the free company.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The Name of the free company.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The current rank of the free company.
        /// </summary>
        public int? Rank { get; set; }

        /// <summary>
        /// The current ranking of the free company.
        /// </summary>
        public Ranking? Ranking { get; set; }

        /// <summary>
        /// The current status of recruitment.
        /// </summary>
        public string? Recruitment { get; set; }

        /// <summary>
        /// The free company's reputation with various Grand Companies.
        /// </summary>
        public List<Reputation>? Reputation { get; set; }

        /// <summary>
        /// What the free company is currently seeking.
        /// </summary>
        public List<Attribute>? Seeking { get; set; }

        /// <summary>
        /// The home server of the free company.
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// The current slogan of the free company.
        /// </summary>
        public string? Slogan { get; set; }

        /// <summary>
        /// The tag of the free company.
        /// </summary>
        /// <remarks>Typically denoted with « » markers, which should be removed in this model.</remarks>
        public string? Tag { get; set; }
    }
}