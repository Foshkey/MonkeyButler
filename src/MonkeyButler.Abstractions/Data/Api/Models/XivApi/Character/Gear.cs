namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character
{
    /// <summary>
    /// A model representing a set of gear on a character.
    /// </summary>
    public record Gear
    {
        /// <summary>
        /// The body gear piece.
        /// </summary>
        public GearPiece? Body { get; set; }

        /// <summary>
        /// The bracelets gear piece.
        /// </summary>
        public GearPiece? Bracelets { get; set; }

        /// <summary>
        /// The earrings gear piece.
        /// </summary>
        public GearPiece? Earrings { get; set; }

        /// <summary>
        /// The feet gear piece.
        /// </summary>
        public GearPiece? Feet { get; set; }

        /// <summary>
        /// The hands gear piece.
        /// </summary>
        public GearPiece? Hands { get; set; }

        /// <summary>
        /// The head gear piece.
        /// </summary>
        public GearPiece? Head { get; set; }

        /// <summary>
        /// The legs gear piece.
        /// </summary>
        public GearPiece? Legs { get; set; }

        /// <summary>
        /// The main-hand gear piece.
        /// </summary>
        public GearPiece? MainHand { get; set; }

        /// <summary>
        /// The necklace gear piece.
        /// </summary>
        public GearPiece? Necklace { get; set; }

        /// <summary>
        /// The off-hand gear piece.
        /// </summary>
        public GearPiece? OffHand { get; set; }

        /// <summary>
        /// The first ring gear piece.
        /// </summary>
        public GearPiece? Ring1 { get; set; }

        /// <summary>
        /// The second ring gear piece.
        /// </summary>
        public GearPiece? Ring2 { get; set; }

        /// <summary>
        /// The soul crystal gear piece.
        /// </summary>
        public GearPiece? SoulCrystal { get; set; }

        /// <summary>
        /// The waist gear piece.
        /// </summary>
        public GearPiece? Waist { get; set; }
    }
}