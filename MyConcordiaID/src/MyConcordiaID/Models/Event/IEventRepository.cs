using System.Collections.Generic;

namespace MyConcordiaID.Models.Event
{
    public interface IEventRepository
    {

        EventActionResult InsertUser(NewEventUser user);
        EventActionResult UpdateUser(EventUser user);
        EventActionResult RemoveUser(EventUser user);
        void InsertEvent(NewEvent newEvent, string Netname);
        EventActionResult UpdateEvent(EventInformation information);
        EventActionResult RemoveEvent(EventCancelled cancellation);
        IEnumerable<EventInformation> GetEvents();
        IEnumerable<EventInformation> GetEventsByStatus(EventStatusType status);
        IEnumerable<EventInformation> GetActiveEvents();
        EventInformation GetEventById(string eventId);
        IEnumerable<EventUserInformation> GetEventUsers(string eventId);
        IEnumerable<dynamic> GetAdminEvents(string netname);
        IEnumerable<AvailableEvent> GetAttendeeEvents(string netname);
        ScannerResult RegisterScannedUser(ScannerUser user);
    }
}
