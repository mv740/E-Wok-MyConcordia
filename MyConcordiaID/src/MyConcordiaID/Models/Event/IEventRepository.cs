using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Event
{
    interface IEventRepository
    {

        ActionResult InsertUser(EventUser user);
        void RemoveUser(EventUser user);
        void InsertEvent(EventInformation information, string Netname);
        void UpdateEvent(EventInformation information);
        void RemoveEvent(EventCancellation cancellation);
        IEnumerable<dynamic> GetEvents();
        dynamic GetEvent(string eventId);
    }
}
