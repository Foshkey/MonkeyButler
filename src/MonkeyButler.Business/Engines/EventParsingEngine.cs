using System;
using System.Collections.Generic;
using System.Linq;
using MonkeyButler.Business.Models.Events;

namespace MonkeyButler.Business.Engines
{
    internal class EventParsingEngine : IEventParsingEngine
    {
        private const string _timeKeyWord = "at";
        private const string _dateKeyWord = "on";
        private const string _todayKeyWord = "today";
        private const string _tomorrowKeyWord = "tomorrow";

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
            var newEvent = new Event()
            {
                CreationDateTime = now
            };

            query = query.Trim(' ', '.', '!', '?');
            var words = query.Split(' ');
            var wordsList = words.ToList();
            now = now.ToOffset(tzOffset);

            var timeKeyIndex = wordsList.FindIndex(x => x.Equals(_timeKeyWord, StringComparison.OrdinalIgnoreCase));
            var dateKeyIndex = wordsList.FindIndex(x => x.Equals(_dateKeyWord, StringComparison.OrdinalIgnoreCase));

            var titleEndIndex = GetEndIndex(timeKeyIndex, dateKeyIndex);
            newEvent.Title = string.Join(' ', words[..titleEndIndex]);

            var timeEndIndex = timeKeyIndex < dateKeyIndex
                ? dateKeyIndex
                : words.Length;

            var dateEndIndex = dateKeyIndex < timeKeyIndex
                ? timeKeyIndex
                : words.Length;

            var timeStr = string.Join(' ', words[(timeKeyIndex + 1)..timeEndIndex]);
            var dateStr = string.Join(' ', words[(dateKeyIndex + 1)..dateEndIndex]);

            var time = DateTimeOffset.TryParse(timeStr, out var timeParsed)
                ? timeParsed
                : FindTimeElsewhere(words);

            var date = DateTimeOffset.TryParse(dateStr, out var dateParsed)
                ? dateParsed
                : FindDateElsewhere(words, dateStr, now);

            newEvent.EventDateTime = Combine(time, date, now, tzOffset)
                ?? throw new InvalidOperationException($"Could not determine event date/time with query '{query}'.");

            return newEvent;
        }

        private int GetEndIndex(int i1, int i2)
        {
            if (i1 < 0)
            {
                return i2;
            }

            if (i2 < 0)
            {
                return i1;
            }

            return i1 < i2 ? i1 : i2;
        }

        private DateTimeOffset? FindTimeElsewhere(string[] words)
        {
            // Strategy for this is try parsing each word, and then sequential pairing of words.
            // Both from right to left, as title is last in priority of parsing.

            foreach (var word in words.Reverse())
            {
                if (DateTimeOffset.TryParse(word, out var time))
                {
                    return time;
                }
            }

            for (var i = words.Length - 2; i >= 0; i--)
            {
                var pair = string.Join(' ', words[i], words[i + 1]);
                if (DateTimeOffset.TryParse(pair, out var time))
                {
                    return time;
                }
            }

            return null;
        }

        private DateTimeOffset? FindDateElsewhere(string[] words, string dateStr, DateTimeOffset now)
        {
            // Check day of week
            if (_dayOfWeekMap.ContainsKey(dateStr))
            {
                return GetNextDay(_dayOfWeekMap[dateStr], now);
            }

            // Day of week in reverse order
            foreach (var word in words.Reverse())
            {
                if (_dayOfWeekMap.ContainsKey(word))
                {
                    return GetNextDay(_dayOfWeekMap[word], now);
                }
            }

            // Tomorrow in reverse order
            foreach (var word in words.Reverse())
            {
                if (string.Equals(_tomorrowKeyWord, word, StringComparison.OrdinalIgnoreCase))
                {
                    return now.AddDays(1);
                }
            }

            return now;
        }

        private DateTimeOffset GetNextDay(DayOfWeek dayOfWeek, DateTimeOffset now)
        {
            var diff = dayOfWeek - now.DayOfWeek;
            if (diff < 0) diff += 7;

            return now.AddDays(diff);
        }

        private DateTimeOffset? Combine(DateTimeOffset? time, DateTimeOffset? date, DateTimeOffset now, TimeSpan offset)
        {
            if (time is null || date is null)
            {
                return null;
            }

            return new DateTimeOffset(
                year: date.Value.Year,
                month: date.Value.Month,
                day: date.Value.Day,
                hour: time.Value.Hour,
                minute: time.Value.Minute,
                second: time.Value.Second,
                offset: offset
            );
        }
    }

    internal interface IEventParsingEngine
    {
        Event Parse(string query, TimeSpan tzOffset);
    }
}
