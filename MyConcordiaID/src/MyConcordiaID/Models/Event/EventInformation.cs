using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Event
{
    public enum TYPE { OPEN, CLOSED};
    
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
