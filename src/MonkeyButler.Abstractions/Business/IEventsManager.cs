using MonkeyButler.Abstractions.Business.Models.Events;

namespace MonkeyButler.Abstractions.Business;

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
