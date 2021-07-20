using System.Collections.Generic;
using System.Text;
using Discord;
using MonkeyButler.Abstractions.Business.Models.Events;

namespace MonkeyButler.Extensions
{
    internal static class EventExtensions
    {
        public static Embed ToEmbed(this Event ev)
        {
            var dateFormat = "dddd, MMM d ⋅ h:mm UTCzzz";

            var timeField = new EmbedFieldBuilder()
            {
                Name = "Time",
                Value = ev.EventDateTime.ToString(dateFormat)
            };

            var rosterField = new EmbedFieldBuilder()
            {
                Name = $"Roster ({ev.Roster.Count})",
                Value = ev.Roster.ToDisplay()
            };

            return new EmbedBuilder()
                .WithTitle(ev.Title)
                .WithFields(timeField, rosterField)
                .Build();
        }

        public static string ToDisplay(this List<RosterEntry> roster)
        {
            var sb = new StringBuilder(">>> ");

            if (roster.Count == 0)
            {
                return sb.AppendLine("-").ToString();
            }

            foreach (var entry in roster)
            {
                foreach (var role in entry.Roles)
                {
                    sb.Append($":{role}:");
                }

                sb.AppendLine($" {entry.Name}");
            }

            return sb.ToString();
        }
    }
}
