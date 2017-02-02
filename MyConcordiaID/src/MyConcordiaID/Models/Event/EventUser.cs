using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Event
{
    public enum Role { Creator, Mod, Scanner, Attendee }

    public class EventUser
    {
        //can use user id or netname to add user to event
        public int? UserId { get; set; }
        public string UserNetname { get; set; }
        public Role Role { get; set; }
        public string EventID { get; set; }
        public TYPE EventType { get; set; }
    }
}
