using System.ComponentModel.DataAnnotations;

namespace MyConcordiaID.Models.Event
{
    //can use user id or netname to add user to event
    public class NewEventUser
    {
        /// <summary>
        /// Use Id or netname
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Use Id or netname
        /// </summary>
        public string UserNetname { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public string EventID { get; set; }
    }
}
