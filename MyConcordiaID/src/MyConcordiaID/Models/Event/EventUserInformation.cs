using MyConcordiaID.Models.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Event
{
    public class EventUserInformation
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public StudentBasicInformation StudentAccount { get; set; }
    }
}
