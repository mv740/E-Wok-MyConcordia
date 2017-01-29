using MyConcordiaID.Models.Picture;
using OracleEntityFramework;
using System;
using System.Linq;
using System.Collections.Generic;

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

            bool updatedPreviousPeriod = false; //logging purposes

            string start = setting.startDate;
            string end = setting.endDate;

            DateTime startDateTime = DateTime.Parse(start);
            DateTime endDateTime = DateTime.Parse(end);

            short year = (short)setting.year;

            var found = _database.PICTUREUPDATESETTINGs
                 .Where(p => p.YEAR == year)
                 .FirstOrDefault();

            if (found == null)
            {
                PICTUREUPDATESETTING updateSetting = new PICTUREUPDATESETTING
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
        public PeriodSetting GetUpdatePicturePeriod()
        {
            // May 1st 2016 start of academic year 2016-17 : summer 2016, fall 16, winter 17

            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;


            int academicYear;
            if (month >= 5)
            {
                academicYear = year;
            }
            else
            {
                academicYear = year - 1;
            }

            var period = _database.PICTUREUPDATESETTINGs
                .Where(p => p.YEAR == academicYear)
                .FirstOrDefault();


            if(period != null)
            {
                PeriodSetting periodFormat = new PeriodSetting
                {
                    year = period.YEAR,
                    startDate = period.STARDATE.ToString(),
                    endDate = period.ENDDATE.ToString()

                   
                };

                return periodFormat;
            }



            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public PICTUREUPDATESETTING GetUpdatePicturePeriod(int year)
        {
            var info = _database.PICTUREUPDATESETTINGs
               .Where(i => i.YEAR == year)
               .SingleOrDefault();

            return info;
        }

        public List<PICTUREUPDATESETTING> GetAllUpdatePicturePeriod()
        {
            var info = _database.PICTUREUPDATESETTINGs
               .ToList();

            return info;
        }

    }
}
