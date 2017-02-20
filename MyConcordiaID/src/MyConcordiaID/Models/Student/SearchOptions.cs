using System.Collections.Generic;

namespace MyConcordiaID.Models.Student
{
    public class SearchOptions
    {
        public int? Id { get; set; }
        public string Netname { get; set; }
        public string Birthdate { get; set; }
        public List<string> Name { get; set; }
    }
}
