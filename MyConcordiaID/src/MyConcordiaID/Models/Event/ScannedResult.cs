using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Event
{

    public enum ScannerStatus { Success, IdNotFound, UserWasNotRegistered} 

    public class ScannerResult
    {
        [Required]
        public ScannerStatus Status { get; set; }
        public string Message { get; set; }
    }
}
