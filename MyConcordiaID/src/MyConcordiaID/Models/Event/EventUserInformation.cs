using MyConcordiaID.Models.Student;

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
