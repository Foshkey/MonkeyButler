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
        private static DateTimeOffset _nowInput = new DateTimeOffset(2020, 6, 20, 16, 0, 0, TimeSpan.Zero);

        private Event Parse(string query, TimeSpan tzOffset) => new SUT().Parse(query, tzOffset, _nowInput);

        [Fact]
        public void NextYearWorks()
        {
            _nowInput = new DateTimeOffset(2020, 12, 31, 16, 0, 0, TimeSpan.Zero);
            var query = "Next year test at 7am.";
            var expectedEvent = new Event()
            {
                Title = "Next year test",
                EventDateTime = _nowInput.AddHours(24 - 5).ToOffset(_tzOffsetInput)
            };

            var result = Parse(query, _tzOffsetInput);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.Equal(_nowInput, result.CreationDateTime);
            Assert.Equal(expectedEvent.EventDateTime, result.EventDateTime);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void StringShouldParse(string query, TimeSpan tzOffset, Event expectedEvent)
        {
            var result = Parse(query, tzOffset);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.Equal(_nowInput, result.CreationDateTime);
            Assert.Equal(expectedEvent.EventDateTime, result.EventDateTime);
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
                "Bunch of periods at 9pm......",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Bunch of periods",
                    EventDateTime = _nowInput.AddHours(9).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Today's dumb stuff at 7:30 am tomorrow.",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Today's dumb stuff",
                    EventDateTime = _nowInput.AddHours(24 - 4.5).ToOffset(_tzOffsetInput)
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
                "Tomorrow is another day at 5pm Monday.",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Tomorrow is another day",
                    EventDateTime = _nowInput.AddHours(24 * 2 + 5).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Monday fun times on Monday at 14:30",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Monday fun times",
                    EventDateTime = _nowInput.AddHours(24 * 2 + 2.5).ToOffset(_tzOffsetInput)
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

            yield return new object[]
            {
                "Weird date parsing on Thursday 2pm",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Weird date parsing",
                    EventDateTime = _nowInput.AddHours(24 * 5 + 2).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Specific date parsing on June 22 at 4pm",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Specific date parsing",
                    EventDateTime = _nowInput.AddHours(24 * 2 + 4).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Maybe I can get this to work tomorrow at 8pm",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Maybe I can get this to work",
                    EventDateTime = _nowInput.AddHours(24 + 8).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Super special case thu 1 pm",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Super special case",
                    EventDateTime = _nowInput.AddHours(24 * 5 + 1).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Rebelling against Tuesday on Monday at 6:15pm",
                _tzOffsetInput,
                new Event()
                {
                    Title = "Rebelling against Tuesday",
                    EventDateTime = _nowInput.AddHours(24 * 2 + 6.25).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Crazy offset at 8am tomorrow",
                TimeSpan.FromHours(7), // now = June 21 1am local
                new Event()
                {
                    Title = "Crazy offset",
                    EventDateTime = _nowInput.AddHours(24 + 7).ToOffset(TimeSpan.FromHours(7))
                }
            };
        }
    }
}
