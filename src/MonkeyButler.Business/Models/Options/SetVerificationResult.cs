using System;
using System.Collections.Generic;
using System.Text;

namespace MonkeyButler.Business.Models.Options
{
    /// <summary>
    /// Result of setting verification.
    /// </summary>
    public class SetVerificationResult
    {
        /// <summary>
        /// The status of setting verification.
        /// </summary>
        public SetVerificationStatus Status { get; set; }
    }
}
