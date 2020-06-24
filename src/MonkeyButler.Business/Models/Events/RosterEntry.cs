using System;
using System.Collections.Generic;

namespace MonkeyButler.Business.Models.Events
{
    /// <summary>
    /// An entry in the roster of an event.
    /// </summary>
    public class RosterEntry
    {
        /// <summary>
        /// Roles of the entry.
        /// </summary>
        public HashSet<string> Roles { get; set; } = new HashSet<string>();

        /// <summary>
        /// Name of the entry.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The Id of the entry's owner.
        /// </summary>
        public ulong OwnerId { get; set; }

        /// <summary>
        /// The date/time of the entry signup.
        /// </summary>
        public DateTimeOffset SignupDateTime { get; set; } = DateTimeOffset.UtcNow;
    }
}
