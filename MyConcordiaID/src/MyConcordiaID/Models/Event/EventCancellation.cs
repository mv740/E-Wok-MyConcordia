using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Event
{
    public enum CancelType { Delete, Cancel};

    public class EventCancellation
    {
        public int EventId { get; set; }
        public CancelType type { get; set; }
    }
}
