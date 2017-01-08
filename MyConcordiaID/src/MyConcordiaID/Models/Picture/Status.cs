using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Picture
{
    /// <summary>
    ///  Pictures have different status available to them
    /// </summary>
    public enum Status
    {
        Pending,
        Approved,
        Denied,
        Archived
    }

}
