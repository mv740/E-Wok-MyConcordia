using OracleEntityFramework;
using System;
using System.Linq;

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

        public PICTUREUPDATESETTING GetUpdatePicturePeriod(int year)
        {
            var info = _database.PICTUREUPDATESETTINGs
               .Where(i => i.YEAR == year)
               .SingleOrDefault();

            return info;
        }

    }
}
