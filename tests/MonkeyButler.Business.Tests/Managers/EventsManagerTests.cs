using System;
using System.Threading.Tasks;
using FluentValidation;
using MonkeyButler.Business.Models.Events;
using Xunit;
using SUT = MonkeyButler.Business.Managers.EventsManager;

namespace MonkeyButler.Business.Tests.Managers
{
    public class EventsManagerTests
    {
        private SUT BuildTarget() => Resolver
            .Resolve<SUT>();

        [Fact]
        public async Task NullCreateCriteriaShouldThrow() => await Assert.ThrowsAsync<ArgumentNullException>(() => BuildTarget().CreateEvent(null!));

        [Fact]
        public async Task InvalidCreateCriteriaShouldThrow()
        {
            var criteria = new CreateEventCriteria()
            {
                Query = ""
            };

            await Assert.ThrowsAsync<ValidationException>(() => BuildTarget().CreateEvent(criteria));
        }

        [Fact]
        public async Task NullUpdateCriteriaShouldThrow() => await Assert.ThrowsAsync<ArgumentNullException>(() => BuildTarget().UpdateEvent(null!));

        [Fact]
        public async Task InvalidUpdateCriteriaShouldThrow()
        {
            var criteria = new UpdateEventCriteria();

            await Assert.ThrowsAsync<ValidationException>(() => BuildTarget().UpdateEvent(criteria));
        }

        [Fact]
        public async Task InvalidEventInUpdateCriteriaShouldThrow()
        {
            var criteria = new UpdateEventCriteria()
            {
                Event = new Event()
            };

            await Assert.ThrowsAsync<ValidationException>(() => BuildTarget().UpdateEvent(criteria));
        }

        [Fact]
        public async Task NullDeleteCriteriaShouldThrow() => await Assert.ThrowsAsync<ArgumentNullException>(() => BuildTarget().DeleteEvent(null!));

        [Fact]
        public async Task InvalidDeleteCriteriaShouldThrow()
        {
            var criteria = new DeleteEventCriteria();

            await Assert.ThrowsAsync<ValidationException>(() => BuildTarget().DeleteEvent(criteria));
        }
    }
}
