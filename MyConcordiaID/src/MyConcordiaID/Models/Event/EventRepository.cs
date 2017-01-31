using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Event
{
    interface EventRepository
    {
        void InsertUser(EventUser user);
        void RemoveUser(EventUser user);
        void InsertEvent(EventInformation information);
        void UpdateEvent(EventInformation information);
        void RemoveEvent(EventCancellation cancellation);
        void GetEvents();
        void GetEvent(int eventId);
    }
}
