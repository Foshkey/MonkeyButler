using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character
{
    /// <summary>
    /// A model representing a gear piece.
    /// </summary>
    public class GearPiece
    {
        /// <summary>
        /// The creator of the gear piece. Null if not crafted.
        /// </summary>
        public long? Creator { get; set; }

        /// <summary>
        /// The current dye of the gear piece. Null if undyed.
        /// </summary>
        public long? Dye { get; set; }

        /// <summary>
        /// The Id of the gear piece.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// The list of materia Ids currently melded to the gear piece.
        /// </summary>
        public List<long>? Materia { get; set; }

        /// <summary>
        /// The id of the gear piece that this gear piece is currently glamoured as.
        /// </summary>
        public long? Mirage { get; set; }
    }
}