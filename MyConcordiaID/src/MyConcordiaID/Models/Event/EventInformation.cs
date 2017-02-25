using System;
using System.ComponentModel.DataAnnotations;

namespace MyConcordiaID.Models.Event
{
    /// <summary>
    ///  Logic result
    /// </summary>
    public enum EventActionResult { Fail, Success, UserNotFound, EventNotFound, UnknownRole,
        DuplicateUser
    }

    /// <summary>
    ///  OPEN : anyone can attend this event
    ///  CLOSED : private event where you need to be registered
    /// </summary>
    public enum EventType { Open, Closed };

    /// <summary>
    /// http://schema.org/EventStatusType
    /// </summary>
    public enum EventStatusType { Cancelled, Postponed, Rescheduled, Scheduled };


    public class EventInformation
    {

        [Required]
        public string EventId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Room { get; set; }

        [Required]
        public DateTime TimeBegin { get; set; }

        [Required]
        public DateTime TimeEnd { get; set; }

        [Required]
        public string Type { get; set; }

        public string Status { get; set; }

    }
}
