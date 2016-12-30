using MyConcordiaID.Models.Admin;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models
{
    public interface IAdminRepository
    {
        bool SetYearUpdatePicturePeriod(PeriodSetting setting);
        List<PICTUREUPDATESETTING> GetAllUpdatePicturePeriod();
        PICTUREUPDATESETTING GetUpdatePicturePeriod();
        PICTUREUPDATESETTING GetUpdatePicturePeriod(int year);
    }
}
