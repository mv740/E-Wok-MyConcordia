using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Picture
{
    public class StudentPictures
    {
        public byte[] profilePicture { get; set; }
        public byte[] pendingPicture { get; set; }
        public IEnumerable<dynamic> archivedPictures { get; set; }
    }
}
