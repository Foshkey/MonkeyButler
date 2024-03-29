﻿using MonkeyButler.Abstractions.Business.Models.Events;

namespace MonkeyButler.Business.Engines;

internal static class EventParsingEngine
{
    private const string _timeKeyWord = "at";
    private const string _dateKeyWord = "on";
    private const string _todayKeyWord = "today";
    private const string _tomorrowKeyWord = "tomorrow";

    private static readonly Dictionary<string, DayOfWeek> _dayOfWeekMap = new(StringComparer.OrdinalIgnoreCase)
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

    private static readonly Dictionary<string, DateTimeOffset> _specialNamesMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["noon"] = new DateTimeOffset(1, 1, 1, 12, 0, 0, TimeSpan.Zero),
        ["midnight"] = new DateTimeOffset(1, 1, 1, 0, 0, 0, TimeSpan.Zero)
    };

    /// <summary>
    /// Parses the query into an event, using the base offset.
    /// </summary>
    /// <param name="query">The string query to parse, standard format is '[Title] on [Date] at [Time]'</param>
    /// <param name="baseOffset">The base offset of the time zone without daylight saving applied. E.g. US Eastern is -5:00 regardless of current time.</param>
    /// <returns>The parsed event.</returns>
    /// <exception cref="InvalidOperationException">Query is malformed and cannot be parsed.</exception>
    public static Event Parse(string query, TimeSpan baseOffset)
    {
        var now = DateTimeOffset.UtcNow;
        return Parse(query, baseOffset, now);
    }

    // Needed to break out "now" for unit testing.
    public static Event Parse(string query, TimeSpan baseOffset, DateTimeOffset now)
    {
        query = query.Trim(' ', '.', '!', '?');
        var words = query.Split(' ');
        var wordsList = words.ToList();
        now = now.ToOffset(baseOffset);

        var timeKeyIndex = wordsList.FindIndex(x => x.Equals(_timeKeyWord, StringComparison.OrdinalIgnoreCase));
        var dateKeyIndex = wordsList.FindIndex(x => x.Equals(_dateKeyWord, StringComparison.OrdinalIgnoreCase));

        var titleEndIndex = GetEndIndex(timeKeyIndex, dateKeyIndex);
        if (titleEndIndex < 0)
        {
            titleEndIndex = words.Length;
        }

        var timeEndIndex = timeKeyIndex < dateKeyIndex
            ? dateKeyIndex
            : words.Length;

        var dateEndIndex = dateKeyIndex < timeKeyIndex
            ? timeKeyIndex
            : words.Length;

        var timeStr = string.Join(' ', words[(timeKeyIndex + 1)..timeEndIndex]);
        var dateStr = string.Join(' ', words[(dateKeyIndex + 1)..dateEndIndex]);

        var containedAm = timeStr.Contains('a', StringComparison.OrdinalIgnoreCase);

        var time = DateTimeOffset.TryParse(timeStr, out var timeParsed)
            ? timeParsed
            : FindTimeElsewhere(words, timeStr, ref titleEndIndex, ref containedAm);

        var date = FindDate(words, dateStr, now, ref titleEndIndex);

        return new Event()
        {
            CreationDateTime = now,
            EventDateTime = Combine(time, date, now, baseOffset, containedAm)
                ?? throw new InvalidOperationException($"Could not determine event date/time with query '{query}'."),
            Title = CreateTitle(words, titleEndIndex)
        };
    }

    private static int GetEndIndex(int i1, int i2)
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

    private static DateTimeOffset? FindTimeElsewhere(string[] words, string timeStr, ref int titleEndIndex, ref bool containedAm)
    {
        // Strategy for this is try parsing each word, and then sequential pairing of words.
        // Both from right to left, as title is last in priority of parsing.

        for (var i = words.Length - 1; i >= 0; i--)
        {
            if (_specialNamesMap.ContainsKey(words[i]))
            {
                if (titleEndIndex > i)
                {
                    titleEndIndex = i;
                }

                return _specialNamesMap[words[i]];
            }

            if (DateTimeOffset.TryParse(words[i], out var time))
            {
                if (titleEndIndex > i)
                {
                    titleEndIndex = i;
                }

                return time;
            }
        }

        for (var i = words.Length - 2; i >= 0; i--)
        {
            var pair = string.Join(' ', words[i], words[i + 1]);
            if (DateTimeOffset.TryParse(pair, out var time))
            {
                containedAm = pair.Contains('a', StringComparison.OrdinalIgnoreCase);

                if (titleEndIndex > i)
                {
                    titleEndIndex = i;
                }

                return time;
            }
        }

        // At this point, see if there's a number that we can find reason out of.
        if (int.TryParse(timeStr, out var timeNum))
        {
            return new DateTimeOffset(1, 1, 1, timeNum, 0, 0, TimeSpan.Zero);
        }

        for (var i = words.Length - 1; i >= 0; i--)
        {
            if (int.TryParse(words[i], out var timeNum2))
            {
                if (titleEndIndex > i)
                {
                    titleEndIndex = i;
                }

                return new DateTimeOffset(1, 1, 1, timeNum2, 0, 0, TimeSpan.Zero);
            }
        }

        return null;
    }

    private static DateTimeOffset? FindDate(string[] words, string dateStr, DateTimeOffset now, ref int titleEndIndex)
    {
        // Check day of week
        if (_dayOfWeekMap.ContainsKey(dateStr))
        {
            return GetNextDay(_dayOfWeekMap[dateStr], now);
        }

        // Day of week in reverse order
        for (var i = words.Length - 1; i >= 0; i--)
        {
            if (_dayOfWeekMap.ContainsKey(words[i]))
            {
                if (titleEndIndex > i)
                {
                    titleEndIndex = i;
                }

                return GetNextDay(_dayOfWeekMap[words[i]], now);
            }
        }

        // Tomorrow in reverse order
        for (var i = words.Length - 1; i >= 0; i--)
        {
            if (string.Equals(_tomorrowKeyWord, words[i], StringComparison.OrdinalIgnoreCase))
            {
                if (titleEndIndex > i)
                {
                    titleEndIndex = i;
                }

                return now.AddDays(1);
            }
        }

        // Finally if it's an actual date
        if (DateTimeOffset.TryParse(dateStr, out var date))
        {
            return date;
        }

        return now;
    }

    private static DateTimeOffset GetNextDay(DayOfWeek dayOfWeek, DateTimeOffset now)
    {
        var diff = dayOfWeek - now.DayOfWeek;
        if (diff <= 0) diff += 7;

        return now.AddDays(diff);
    }

    private static string CreateTitle(string[] words, int titleEndIndex)
    {
        // Knock off last word of the title if it's "today" or "tomorrow"
        if (string.Equals(_todayKeyWord, words[titleEndIndex - 1], StringComparison.OrdinalIgnoreCase) ||
            string.Equals(_tomorrowKeyWord, words[titleEndIndex - 1], StringComparison.OrdinalIgnoreCase))
        {
            titleEndIndex--;
        }

        return string.Join(' ', words[..titleEndIndex]);
    }

    private static DateTimeOffset? Combine(DateTimeOffset? time, DateTimeOffset? date, DateTimeOffset now, TimeSpan offset, bool containedAm)
    {
        if (time is null || date is null)
        {
            return null;
        }

        var dateTime = Combine(time.Value, date.Value, offset);

        // Since the offset we've been working is the base offset, check for daylight saving
        if (GetEasternTimeZone().IsDaylightSavingTime(dateTime))
        {
            offset += TimeSpan.FromHours(1);
            dateTime = new DateTimeOffset(dateTime.DateTime, offset);
        }

        // If before current time
        if (dateTime <= now)
        {
            // If the time "words" didn't contain AM but less than 12
            if (!containedAm && time.Value.Hour <= 12)
            {
                // Perhaps they meant pm
                dateTime = Combine(time.Value.AddHours(12), date.Value, offset);
                if (dateTime <= now)
                {
                    // Nope, just add a day.
                    dateTime = Combine(time.Value, date.Value.AddDays(1), offset);
                }
            }
            else
            {
                // It was absolutely defined.
                dateTime = Combine(time.Value, date.Value.AddDays(1), offset);
            }
        }

        return dateTime;
    }

    private static DateTimeOffset Combine(DateTimeOffset time, DateTimeOffset date, TimeSpan offset)
    {
        return new DateTimeOffset(
            year: date.Year,
            month: date.Month,
            day: date.Day,
            hour: time.Hour,
            minute: time.Minute,
            second: time.Second,
            offset: offset
        );
    }

    private static TimeZoneInfo GetEasternTimeZone()
    {
        try
        {
            // Windows
            return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            // Linux/Mac
            // If this throws an exception, let it throw.
            return TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        }
    }
}
