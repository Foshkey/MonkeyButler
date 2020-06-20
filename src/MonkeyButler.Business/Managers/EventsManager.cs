using System.Threading.Tasks;
using FluentValidation;
using MonkeyButler.Business.Models.Events;
using MonkeyButler.Business.Validators.Events;

namespace MonkeyButler.Business.Managers
{
    internal class EventsManager : IEventsManager
    {
        public async Task<CreateEventResult> CreateEvent(CreateEventCriteria criteria)
        {
            new CreateEventValidator().ValidateAndThrow(criteria);

            return new CreateEventResult();
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
