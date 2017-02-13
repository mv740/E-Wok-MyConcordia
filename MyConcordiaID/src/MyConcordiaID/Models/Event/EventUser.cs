using System.ComponentModel.DataAnnotations;

namespace MyConcordiaID.Models.Event
{
    public enum Role { Creator, Mod, Scanner, Attendee }

    /// <summary>
    ///  Tracking : used for statistic purpose only
    ///  Registered/Attending : used for closed event 
    ///  
    /// </summary>
    public enum UserStatus { Registered, Attending, Tracking, EventOrganizer }

    public class EventUser
    {

        [Required]
        public string UserId { get; set; }

        public Role Role { get; set; }

    }
}
