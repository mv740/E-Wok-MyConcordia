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
        Task<List<PeriodSetting>> GetAllUpdatePicturePeriodAsync();
        Task<PeriodSetting> GetUpdatePicturePeriodAsync();
        Task<PeriodSetting> GetUpdatePicturePeriodAsync(int year);
    }
}
