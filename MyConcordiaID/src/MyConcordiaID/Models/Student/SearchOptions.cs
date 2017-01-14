using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Student
{
    public class SearchOptions
    {
        public int? id { get; set; }
        public string netname { get; set; }
        public string birthdate { get; set; }
        public List<string> name { get; set; }
    }
}
