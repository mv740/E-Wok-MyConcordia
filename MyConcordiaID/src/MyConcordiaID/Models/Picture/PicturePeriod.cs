using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Picture
{
    public class PicturePeriod
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool canUpdatePicture { get; set; }
    }
}
