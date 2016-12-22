using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Graduation
{
    public interface IGraduationRepository
    {
        GraduationStatus GetMarshallingCard(string netName);
        GraduationStatus GetMarshallingCardRequestDenied(string netName);
    }
}
