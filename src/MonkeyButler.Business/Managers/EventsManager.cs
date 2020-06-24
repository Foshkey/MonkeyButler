using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.Events;
using MonkeyButler.Business.Validators.Events;

namespace MonkeyButler.Business.Managers
{
    internal class EventsManager : IEventsManager
    {
        private readonly IEventParsingEngine _eventParsingEngine;
        private readonly ILogger<EventsManager> _logger;

        public EventsManager(IEventParsingEngine eventParsingEngine, ILogger<EventsManager> logger)
        {
            _eventParsingEngine = eventParsingEngine ?? throw new ArgumentNullException(nameof(eventParsingEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<CreateEventResult> CreateEvent(CreateEventCriteria criteria)
        {
            new CreateEventValidator().ValidateAndThrow(criteria);

            Event newEvent;

            try
            {
                newEvent = _eventParsingEngine.Parse(criteria.Query, new TimeSpan(-5, 0, 0));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error in creating event with query '{Query}'.", criteria.Query);
                return Task.FromResult(new CreateEventResult()
                {
                    IsSuccessful = false
                });
            }

            return Task.FromResult(new CreateEventResult()
            {
                Event = newEvent,
                IsSuccessful = true
            });
        }

        public async Task<UpdateEventResult> UpdateEvent(UpdateEventCriteria criteria)
        {
            new UpdateEventValidator().ValidateAndThrow(criteria);

            return new UpdateEventResult();
        }

        public async Task<DeleteEventResult> DeleteEvent(DeleteEventCriteria criteria)
        {
            new DeleteEventValidator().ValidateAndThrow(criteria);

            return new DeleteEventResult();
        }
    }

    /// <summary>
    /// The manager for managing FC events.
    /// </summary>
    public interface IEventsManager
    {
        /// <summary>
        /// Creates a brand new event.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<CreateEventResult> CreateEvent(CreateEventCriteria criteria);

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<UpdateEventResult> UpdateEvent(UpdateEventCriteria criteria);

        /// <summary>
        /// Deletes an event, if it exists.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<DeleteEventResult> DeleteEvent(DeleteEventCriteria criteria);
    }
}
