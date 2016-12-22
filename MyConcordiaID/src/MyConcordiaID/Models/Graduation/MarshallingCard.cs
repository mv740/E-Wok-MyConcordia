using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Graduation
{
    public class MarshallingCard
    {
        public string Semester { get; set; }
        public int Year { get; set; }
        public string MarshallingCode { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string Degree { get; set; }
        public int SID { get; set; }

    }
}
