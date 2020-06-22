using System;
using System.Collections.Generic;
using MonkeyButler.Business.Models.Events;

namespace MonkeyButler.Business.Engines
{
    internal class EventParsingEngine : IEventParsingEngine
    {
        private const string _timeKeyWord = "at";
        private const string _dateKeyWord = "on";

        private readonly Dictionary<string, DayOfWeek> _dayOfWeekMap = new Dictionary<string, DayOfWeek>(StringComparer.OrdinalIgnoreCase)
        {
            ["monday"] = DayOfWeek.Monday,
            ["mon"] = DayOfWeek.Monday,
            ["tuesday"] = DayOfWeek.Tuesday,
            ["tues"] = DayOfWeek.Tuesday,
            ["tue"] = DayOfWeek.Tuesday,
            ["wednesday"] = DayOfWeek.Wednesday,
            ["wed"] = DayOfWeek.Wednesday,
            ["thursday"] = DayOfWeek.Thursday,
            ["thurs"] = DayOfWeek.Thursday,
            ["thur"] = DayOfWeek.Thursday,
            ["thu"] = DayOfWeek.Thursday,
            ["friday"] = DayOfWeek.Friday,
            ["fri"] = DayOfWeek.Friday,
            ["saturday"] = DayOfWeek.Saturday,
            ["sat"] = DayOfWeek.Saturday,
            ["sunday"] = DayOfWeek.Sunday,
            ["sun"] = DayOfWeek.Sunday
        };

        public Event Parse(string query, TimeSpan tzOffset)
        {
            var now = DateTimeOffset.UtcNow;
            return Parse(query, tzOffset, now);
        }

        // Needed to break out "now" for unit testing.
        public Event Parse(string query, TimeSpan tzOffset, DateTimeOffset now)
        {
            var newEvent = new Event();
            var words = query.Split(' ');

            var timeKeyIndex = Array.IndexOf(words, _timeKeyWord);
            var dateKeyIndex = Array.IndexOf(words, _dateKeyWord);

            if (timeKeyIndex < 0 && dateKeyIndex < 0)
            {
                throw new InvalidOperationException($"Query '{query}' not formed properly. Could not find key words 'at' or 'on'.");
            }



            return newEvent;
        }
    }

    internal interface IEventParsingEngine
    {
        Event Parse(string query, TimeSpan tzOffset);
    }
}
