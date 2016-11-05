using MyConcordiaID.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models
{
    public interface IAdminRepository
    {
        void SetYearUpdatePicturePeriod(PeriodSetting setting);
    }
}
