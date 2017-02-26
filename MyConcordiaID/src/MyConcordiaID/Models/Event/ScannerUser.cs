using System.ComponentModel.DataAnnotations;

namespace MyConcordiaID.Models.Event
{
    public class ScannerUser
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public EventType Type { get; set; }

        [Required]
        public string EventID { get; set; }


    }
}
