using System;
using System.Threading.Tasks;
using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Events;
using MonkeyButler.Business.Managers;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers
{
    public class EventsManagerTests
    {
        private EventsManager _manager => Resolver
            .Resolve<EventsManager>();

        [Fact]
        public async Task NullCreateCriteriaShouldThrow() => await Assert.ThrowsAsync<ArgumentNullException>(() => _manager.CreateEvent(null!));

        [Fact]
        public async Task InvalidCreateCriteriaShouldThrow()
        {
            var criteria = new CreateEventCriteria()
            {
                Query = ""
            };

            await Assert.ThrowsAsync<ValidationException>(() => _manager.CreateEvent(criteria));
        }

        [Fact]
        public async Task NullUpdateCriteriaShouldThrow() => await Assert.ThrowsAsync<ArgumentNullException>(() => _manager.UpdateEvent(null!));

        [Fact]
        public async Task InvalidUpdateCriteriaShouldThrow()
        {
            var criteria = new UpdateEventCriteria();

            await Assert.ThrowsAsync<ValidationException>(() => _manager.UpdateEvent(criteria));
        }

        [Fact]
        public async Task InvalidEventInUpdateCriteriaShouldThrow()
        {
            var criteria = new UpdateEventCriteria()
            {
                Event = new Event()
            };

            await Assert.ThrowsAsync<ValidationException>(() => _manager.UpdateEvent(criteria));
        }

        [Fact]
        public async Task NullDeleteCriteriaShouldThrow() => await Assert.ThrowsAsync<ArgumentNullException>(() => _manager.DeleteEvent(null!));

        [Fact]
        public async Task InvalidDeleteCriteriaShouldThrow()
        {
            var criteria = new DeleteEventCriteria();

            await Assert.ThrowsAsync<ValidationException>(() => _manager.DeleteEvent(criteria));
        }
    }
}
