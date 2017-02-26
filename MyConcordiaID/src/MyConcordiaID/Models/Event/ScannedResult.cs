using System.ComponentModel.DataAnnotations;

namespace MyConcordiaID.Models.Event
{

    public enum ScannerStatus { Success, IdNotFound, UserWasNotRegistered} 

    public class ScannerResult
    {
        [Required]
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
