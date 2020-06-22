using System;
using System.Collections.Generic;
using MonkeyButler.Business.Models.Events;
using Xunit;
using SUT = MonkeyButler.Business.Engines.EventParsingEngine;

namespace MonkeyButler.Business.Tests.Engine
{
    public class EventParsingEngineTests
    {
        // Now = Saturday, June 20, Noon EDT
        private static readonly TimeSpan _tzOffsetInput = TimeSpan.FromHours(-4);
        private static readonly DateTimeOffset _nowInput = new DateTimeOffset(2020, 6, 20, 16, 0, 0, TimeSpan.Zero);

        private Event Parse(string query, TimeSpan tzOffset) => new SUT().Parse(query, tzOffset, _nowInput);

        [Theory]
        [MemberData(nameof(TestData))]
        public void StringShouldParse(string query, TimeSpan tzOffset, Event expectedEvent)
        {
            var result = Parse(query, tzOffset);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.True(expectedEvent.EventDateTime.Equals(result.EventDateTime));
        }

        private static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                "Hello World at 8pm.",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Hello World",
                    EventDateTime = _nowInput.AddHours(8).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Dumb stuff at 7am.",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Dumb stuff",
                    EventDateTime = _nowInput.AddHours(24 - 5).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Tomorrow's homework at 3pm tomorrow",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Tomorrow's homework",
                    EventDateTime = _nowInput.AddHours(24 + 3).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Monday fun times on Monday at 14:30",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Monday fun times",
                    EventDateTime = _nowInput.AddHours(24 + 24 + 2.5).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Saturday morning beats on Saturday at 9am",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Saturday morning beats",
                    EventDateTime = _nowInput.AddHours(24 * 7 - 3).ToOffset(_tzOffsetInput)
                }
            };
        }
    }
}
