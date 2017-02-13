using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
