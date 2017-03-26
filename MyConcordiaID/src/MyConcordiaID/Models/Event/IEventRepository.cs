using System.Collections.Generic;
using System.Threading.Tasks;
using MyConcordiaID.Models.Event.Statistic;

namespace MyConcordiaID.Models.Event
{
    public interface IEventRepository
    {

        Task<EventActionResult> InsertUserAsync(NewEventUser user);
        Task<EventActionResult> UpdateUserAsync(EventUser user);
        Task<EventActionResult> RemoveUserAsync(EventUser user);
        void InsertEvent(NewEvent newEvent, string Netname);
        Task<EventActionResult> UpdateEventAsync(EventInformation information);
        Task<EventActionResult> RemoveEventAsync(EventCancelled cancellation);
        Task<IEnumerable<EventInformation>> GetEventsAsync();
        Task<IEnumerable<EventInformation>> GetEventsByStatusAsync(EventStatusType status);
        Task<IEnumerable<EventInformation>> GetActiveEventsAsync();
        Task<EventInformation> GetEventByIdAsync(string eventId);
        Task<IEnumerable<EventUserInformation>> GetEventUsersAsync(string eventId, bool orderUserOnTop, string mWoznia);
        Task<IEnumerable<AvailableEvent>> GetAdminEventsAsync(string netname);
        Task<IEnumerable<AvailableEvent>> GetAttendeeEventsAsync(string netname);
        ScannerResult RegisterScannedUser(ScannerUser user);
        Task<EventStatistic> GetEventStatisticAsync(string eventId);
    }
}
