using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Event
{
    /// <summary>
    ///  Logic result
    /// </summary>
    public enum ActionResult { Fail, Success, UserNotFound }

    /// <summary>
    ///  OPEN : anyone can attend this event
    ///  CLOSED : private event where you need to be registered
    /// </summary>
    public enum TYPE { OPEN, CLOSED};

    /// <summary>
    ///  Tracking : used for statistic purpose only
    ///  Registered/Attending : used for closed event 
    ///  
    /// </summary>
    public enum Status { Registered, Attending, Tracking, EventOrganizer}
    
    public class EventInformation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Room { get; set; }
        public string TimeBegin { get; set; }
        public string TimeEnd { get; set; }
        public TYPE Type  { get; set; }

    }
}
