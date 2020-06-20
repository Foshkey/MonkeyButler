using System.Collections.Generic;

namespace MonkeyButler.Business.Models.Events
{
    /// <summary>
    /// A model of the roster on an event.
    /// </summary>
    public class Roster
    {
        /// <summary>
        /// The entries in a roster, keyed the entry's owner Id.
        /// </summary>
        public Dictionary<long, RosterEntry> Entries { get; set; } = new Dictionary<long, RosterEntry>();
    }
}
