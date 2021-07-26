﻿using System;

namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi
{
    /// <summary>
    /// A parent model for XIV API models.
    /// </summary>
    public class XivApiModel
    {
        /// <summary>
        /// The datetime that the model was last parsed.
        /// </summary>
        public DateTimeOffset? ParseDate { get; set; }
    }
}