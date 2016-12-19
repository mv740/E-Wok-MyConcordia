using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OracleEntityFramework;

namespace MyConcordiaID.Models.Log
{
    public class LogRepository : ILogRepository
    {

        private readonly DatabaseEntities _database;

        public LogRepository(DatabaseEntities context)
        {
            _database = context;
        }


        public IEnumerable<dynamic> GetLalestLogs(int count)
        {
            var logs = _database.LOGs
                .Select(log => new
                {
                    log.ID_PK,
                    log.NETNAME,
                    log.ACTION,
                    log.TIMESTAMP
                })
                .OrderByDescending(log => log.ID_PK)
                .Take(count);

            return logs;


            throw new NotImplementedException();
        }

        /// <summary>
        /// Keep a log of every action 
        /// </summary>
        /// <param name="netname"></param>
        /// <param name="action"></param>
        public void Logger(string netname, Log.Action action)
        {

            LOG currentLog = new LOG
            {
                NETNAME = netname,
                ACTION = action.ToString(),
                TIMESTAMP = DateTime.UtcNow
     
            };

            _database.LOGs.Add(currentLog);
            _database.SaveChanges();
        }

    }
}
