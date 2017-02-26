using OracleEntityFramework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using MyConcordiaID.Helper;

namespace MyConcordiaID.Models.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DatabaseEntities _database;

        public AdminRepository(DatabaseEntities context)
        {
            _database = context;
        }
        /// <summary>
        ///  Set a valid period where student get update their valid profile picture 
        /// </summary>
        /// <param name="setting"></param>
        public bool SetYearUpdatePicturePeriod(PeriodSetting setting)
        {
            var updatedPreviousPeriod = false; //logging purposes

            var startDateTime = FormatHelper.ConvertToDateTime(setting.StartDate);
            var endDateTime = FormatHelper.ConvertToDateTime(setting.EndDate);

            var year = (short)setting.Year;

            var found = _database.PICTUREUPDATESETTINGs
                .FirstOrDefault(p => p.YEAR == year);

            if (found == null)
            {
                var updateSetting = new PICTUREUPDATESETTING
                {
                    YEAR = year,
                    STARDATE = startDateTime,
                    ENDDATE = endDateTime
                };

                _database.PICTUREUPDATESETTINGs.Add(updateSetting);
            }
            else
            {
                //else update values
                found.STARDATE = startDateTime;
                found.ENDDATE = endDateTime;
                updatedPreviousPeriod = true;
            }

            var allStudents = _database.STUDENTS.ToList();

            allStudents.ForEach(s => s.UPDATEPICTURE = true);

            _database.SaveChanges();

            return updatedPreviousPeriod;
        }

        /// <summary>
        ///  Get current Academic Update picture period
        /// </summary>
        /// <returns></returns>
        public async Task<PeriodSetting> GetUpdatePicturePeriodAsync()
        {
            // May 1st 2016 start of academic year 2016-17 : summer 2016, fall 16, winter 17
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            const int newAcademicYearMonth = 5;

            int academicYear;
            if (currentMonth >= newAcademicYearMonth)
            {
                academicYear = currentYear;
            }
            else
            {
                academicYear = currentYear - 1;
            }

            //Get require no tracking
            var period = await _database.PICTUREUPDATESETTINGs
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.YEAR == academicYear);


            if (period != null)
            {
                var periodFormat = new PeriodSetting
                {
                    Year = period.YEAR,
                    StartDate = period.STARDATE.ToString(),
                    EndDate = period.ENDDATE.ToString()
                };

                return periodFormat;
            }
            return null;
        }

        /// <summary>
        ///  Retrieve an academic update period 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<PeriodSetting> GetUpdatePicturePeriodAsync(int year)
        {
            var period = await _database.PICTUREUPDATESETTINGs
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.YEAR == year);

            if (period != null)
            {
                var periodFormat = new PeriodSetting
                {
                    Year = period.YEAR,
                    StartDate = period.STARDATE.ToString(),
                    EndDate = period.ENDDATE.ToString()
                };

                return periodFormat;
            }
            return null;
        }

        /// <summary>
        /// Retrieve a list of all update periods
        /// </summary>
        /// <returns></returns>
        public async Task<List<PeriodSetting>> GetAllUpdatePicturePeriodAsync()
        {
            var info = await _database.PICTUREUPDATESETTINGs
                .AsNoTracking()
                .ToListAsync();

            var periodList = info.Select(period => new PeriodSetting
            {
                Year = period.YEAR,
                StartDate = period.STARDATE.ToString(),
                EndDate = period.ENDDATE.ToString()
            }).ToList();

            return periodList;
        }

    }
}
