﻿using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DatabaseEntities _database;

        public AdminRepository(DatabaseEntities context)
        {
            _database = context;
        }

        public void SetYearUpdatePicturePeriod(PeriodSetting setting)
        {

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
            }

            _database.SaveChanges();



        }
    }
}
