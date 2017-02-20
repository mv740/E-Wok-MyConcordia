using System.Collections.Generic;

namespace MyConcordiaID.Models.Picture
{
    public class StudentPictures
    {
        public dynamic ProfilePicture { get; set; }
        public dynamic PendingPicture { get; set; }
        public IEnumerable<dynamic> ArchivedPictures { get; set; }
    }
}
