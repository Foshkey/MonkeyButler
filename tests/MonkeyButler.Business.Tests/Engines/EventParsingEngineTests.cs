using System;
using System.Collections.Generic;
using MonkeyButler.Abstractions.Business.Models.Events;
using MonkeyButler.Business.Engines;
using Xunit;

namespace MonkeyButler.Business.Tests.Engines
{
    public class EventParsingEngineTests
    {
        // Now = Saturday, June 20, Noon EDT
        private static readonly TimeSpan _tzOffsetInput = TimeSpan.FromHours(-5);
        private static readonly DateTimeOffset _nowInput = new DateTimeOffset(2020, 6, 20, 16, 0, 0, TimeSpan.Zero);

        [Fact]
        public void NextYearWorks()
        {
            var now = new DateTimeOffset(2020, 12, 31, 17, 0, 0, TimeSpan.Zero);
            var query = "Next year test at 7am.";
            var expectedEvent = new Event()
            {
                Title = "Next year test",
                EventDateTime = now.AddHours(24 - 5).ToOffset(_tzOffsetInput)
            };

            var result = EventParsingEngine.Parse(query, _tzOffsetInput, now);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.Equal(now, result.CreationDateTime);
            Assert.Equal(expectedEvent.EventDateTime, result.EventDateTime);
        }

        [Fact]
        public void EventAt1pmWhenNowIs2pmShouldSetEventDayAfter()
        {
            var now = new DateTimeOffset(2020, 6, 20, 18, 0, 0, TimeSpan.Zero); // 2pm EDT
            var query = "Event at 1pm.";
            var expectedEvent = new Event()
            {
                Title = "Event",
                EventDateTime = now.AddHours(24 - 1).ToOffset(_tzOffsetInput)
            };

            var result = EventParsingEngine.Parse(query, _tzOffsetInput, now);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.Equal(now, result.CreationDateTime);
            Assert.Equal(expectedEvent.EventDateTime, result.EventDateTime);
        }

        [Fact]
        public void UnusualTimeZoneShouldStillWork()
        {
            // now = June 21 1am local
            var offset = TimeSpan.FromHours(9);
            var query = "Crazy offset at 8am tomorrow";
            var expectedEvent = new Event()
            {
                Title = "Crazy offset",
                EventDateTime = _nowInput.AddHours(24 + 6).ToOffset(offset)
            };

            var result = EventParsingEngine.Parse(query, offset, _nowInput);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.Equal(_nowInput, result.CreationDateTime);
            Assert.Equal(expectedEvent.EventDateTime, result.EventDateTime);
        }

        [Fact]
        public void CreatingEventInsideDaylightSavingShouldWork()
        {
            var now = new DateTimeOffset(2020, 3, 6, 17, 0, 0, TimeSpan.Zero); // 12pm EST (not DST)
            var query = "Daylight Saving Celebration on Sunday 10am";
            var expectedEvent = new Event()
            {
                Title = "Daylight Saving Celebration",
                EventDateTime = now.AddHours(24 * 2 - 3).ToOffset(_tzOffsetInput) // 10am EDT
            };

            var result = EventParsingEngine.Parse(query, _tzOffsetInput, now);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.Equal(now, result.CreationDateTime);
            Assert.Equal(expectedEvent.EventDateTime, result.EventDateTime);
        }

        [Fact]
        public void CreatingEventOutsideDaylightSavingShouldWork()
        {
            var now = new DateTimeOffset(2020, 10, 30, 16, 0, 0, TimeSpan.Zero); // 12pm EDT
            var query = "Daylight Saving End Celebration on Sunday 10am";
            var expectedEvent = new Event()
            {
                Title = "Daylight Saving End Celebration",
                EventDateTime = now.AddHours(24 * 2 - 1).ToOffset(_tzOffsetInput) // 10am EST (not DST)
            };

            var result = EventParsingEngine.Parse(query, _tzOffsetInput, now);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.Equal(now, result.CreationDateTime);
            Assert.Equal(expectedEvent.EventDateTime, result.EventDateTime);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void StringShouldParse(string query, Event expectedEvent)
        {
            var result = EventParsingEngine.Parse(query, _tzOffsetInput, _nowInput);

            Assert.Equal(expectedEvent.Title, result.Title);
            Assert.Equal(_nowInput, result.CreationDateTime);
            Assert.Equal(expectedEvent.EventDateTime, result.EventDateTime);
        }

        private static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                "Hello World at 8pm.",
                new Event()
                {
                    Title = "Hello World",
                    EventDateTime = _nowInput.AddHours(8).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Bunch of periods at 9pm......",
                new Event()
                {
                    Title = "Bunch of periods",
                    EventDateTime = _nowInput.AddHours(9).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Early morning jams at 9am",
                new Event()
                {
                    Title = "Early morning jams",
                    EventDateTime = _nowInput.AddHours(24 - 3).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Some stuff today at 3",
                new Event()
                {
                    Title = "Some stuff",
                    EventDateTime = _nowInput.AddHours(3).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Half-hour stuff at 3:30",
                new Event()
                {
                    Title = "Half-hour stuff",
                    EventDateTime = _nowInput.AddHours(3.5).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Today's dumb stuff at 7:30 am tomorrow.",
                new Event()
                {
                    Title = "Today's dumb stuff",
                    EventDateTime = _nowInput.AddHours(24 - 4.5).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Other dumb stuff at 6:30 tomorrow.",
                new Event()
                {
                    Title = "Other dumb stuff",
                    EventDateTime = _nowInput.AddHours(24 - 5.5).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Tomorrow's homework at 3pm tomorrow",
                new Event()
                {
                    Title = "Tomorrow's homework",
                    EventDateTime = _nowInput.AddHours(24 + 3).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Tomorrow is another day at 5pm Monday.",
                new Event()
                {
                    Title = "Tomorrow is another day",
                    EventDateTime = _nowInput.AddHours(24 * 2 + 5).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Monday fun times on Monday at 14:30",
                new Event()
                {
                    Title = "Monday fun times",
                    EventDateTime = _nowInput.AddHours(24 * 2 + 2.5).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Saturday morning beats on Saturday at 9am",
                new Event()
                {
                    Title = "Saturday morning beats",
                    EventDateTime = _nowInput.AddHours(24 * 7 - 3).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Weird date parsing on Thursday 2pm",
                new Event()
                {
                    Title = "Weird date parsing",
                    EventDateTime = _nowInput.AddHours(24 * 5 + 2).ToOffset(_tzOffsetInput)
                }
            };

            // Using DateTimeOffset.Parse here due to using the same in engine
            var june22 = DateTimeOffset.Parse("June 22");
            yield return new object[]
            {
                "Specific date parsing on June 22 at 4pm",
                new Event()
                {
                    Title = "Specific date parsing",
                    EventDateTime = new DateTimeOffset(
                        june22.Year,
                        june22.Month,
                        june22.Day,
                        16, 0, 0, _tzOffsetInput.Add(TimeSpan.FromHours(1))) // Extra hour for DST
                }
            };

            yield return new object[]
            {
                "Maybe I can get this to work tomorrow at 8pm",
                new Event()
                {
                    Title = "Maybe I can get this to work",
                    EventDateTime = _nowInput.AddHours(24 + 8).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Super special case thu 1 pm",
                new Event()
                {
                    Title = "Super special case",
                    EventDateTime = _nowInput.AddHours(24 * 5 + 1).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Rebelling against Tuesday on Monday at 6:15pm",
                new Event()
                {
                    Title = "Rebelling against Tuesday",
                    EventDateTime = _nowInput.AddHours(24 * 2 + 6.25).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Lunch 12pm",
                new Event()
                {
                    Title = "Lunch",
                    EventDateTime = _nowInput.AddHours(24).ToOffset(_tzOffsetInput)
                }
            };

            yield return new object[]
            {
                "Lunch at noon",
                new Event()
                {
                    Title = "Lunch",
                    EventDateTime = _nowInput.AddHours(24).ToOffset(_tzOffsetInput)
                }
            };
        }
    }
}
