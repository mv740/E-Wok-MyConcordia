using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Student
{
    public class StudentAccount
    {
        public string NetName { get; set; }
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Valid { get; set; }
        public DateTime DOB { get; set; }
        public bool Pending { get; set; }
        public string UGradStatus { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool UpdatePicture { get; set; }
        public byte[] ProfilePicture { get; set; }
        public byte[] PendingPicture { get; set; }

    }
}
