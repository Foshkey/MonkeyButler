using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.Events;

namespace MonkeyButler.Business.Managers
{
    internal class EventsManager : IEventsManager
    {
        private readonly IEventParsingEngine _eventParsingEngine;
        private readonly ILogger<EventsManager> _logger;
        private readonly IValidator<CreateEventCriteria> _createValidator;
        private readonly IValidator<SaveEventCriteria> _saveValidator;
        private readonly IValidator<UpdateEventCriteria> _updateValidator;
        private readonly IValidator<DeleteEventCriteria> _deleteValidator;

        public EventsManager(
            IEventParsingEngine eventParsingEngine,
            ILogger<EventsManager> logger,
            IValidator<CreateEventCriteria> createValidator,
            IValidator<SaveEventCriteria> saveValidator,
            IValidator<UpdateEventCriteria> updateValidator,
            IValidator<DeleteEventCriteria> deleteValidator)
        {
            _eventParsingEngine = eventParsingEngine ?? throw new ArgumentNullException(nameof(eventParsingEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _saveValidator = saveValidator ?? throw new ArgumentNullException(nameof(saveValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        }

        public Task<CreateEventResult> CreateEvent(CreateEventCriteria criteria)
        {
            _createValidator.ValidateAndThrow(criteria);

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

        public async Task<SaveEventResult> SaveEvent(SaveEventCriteria criteria)
        {
            _saveValidator.ValidateAndThrow(criteria);

            return new SaveEventResult()
            {
                IsSuccessful = true
            };
        }

        public async Task<UpdateEventResult> UpdateEvent(UpdateEventCriteria criteria)
        {
            _updateValidator.ValidateAndThrow(criteria);

            return new UpdateEventResult();
        }

        public async Task<DeleteEventResult> DeleteEvent(DeleteEventCriteria criteria)
        {
            _deleteValidator.ValidateAndThrow(criteria);

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
        /// Creates a brand new event.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<SaveEventResult> SaveEvent(SaveEventCriteria criteria);

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
