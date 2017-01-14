using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Picture
{
    public class StudentPictures
    {
        public dynamic profilePicture { get; set; }
        public dynamic pendingPicture { get; set; }
        public IEnumerable<dynamic> archivedPictures { get; set; }
    }
}
