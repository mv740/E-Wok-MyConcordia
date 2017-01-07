using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OracleEntityFramework;
using System.Data.Entity;

namespace MyConcordiaID.Models.Log
{
    public class LogRepository : ILogRepository
    {

        private readonly DatabaseEntities _database;

        public LogRepository(DatabaseEntities context)
        {
            _database = context;
        }


        public async Task<IEnumerable<dynamic>> GetLalestLogs(int count)
        {
            var logs = await _database.LOGs
                .Select(log => new
                {
                    log.NETNAME,
                    log.ACTION,
                    log.AFFECTED_USER,
                    log.TIMESTAMP
                })
                .OrderByDescending(log => log.TIMESTAMP)
                .Take(count)
                .ToListAsync();

            return logs;
        }

        public async Task<IEnumerable<dynamic>> GetStudentLogs(string netName)
        {
            var logs = await _database.LOGs
                .Where(s => s.NETNAME == netName || s.AFFECTED_USER == netName)
                .Select(log => new
                {
                    log.NETNAME,
                    log.ACTION,
                    log.AFFECTED_USER,
                    log.TIMESTAMP
                })
                .OrderByDescending(log => log.TIMESTAMP)
                .Take(10)
                .ToListAsync();

            return logs;
        }

        /// <summary>
        /// Keep a log of every action 
        /// </summary>
        /// <param name="netname"></param>
        /// <param name="action"></param>
        /// <param name="affectedUser"></param>
        public void Logger(string netname, Log.Action action, string affectedUser)
        {

           
            LOG currentLog = new LOG
            {
                NETNAME = netname,
                ACTION = action.ToString(),
                TIMESTAMP = DateTime.UtcNow,
                AFFECTED_USER = affectedUser

            };

            _database.LOGs.Add(currentLog);
            _database.SaveChangesAsync();
        }

    }
}
