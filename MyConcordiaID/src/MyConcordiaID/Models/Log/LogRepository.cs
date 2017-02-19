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


        /// <summary>
        /// Get lastest logs, take only {count} ammount 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetLalestLogsAsync(int count)
        {
            var logs = await _database.LOGs
                .AsNoTracking()
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


        /// <summary>
        ///  Retrieve latest 10 logs related to a specific user
        /// </summary>
        /// <param name="netName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetStudentLogsAsync(string netName)
        {
            var logs = await _database.LOGs
                .AsNoTracking()
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
        public async Task LoggerAsync(string netname, Log.Action action, string affectedUser)
        {
            var currentLog = new LOG
            {
                NETNAME = netname,
                ACTION = action.ToString(),
                TIMESTAMP = DateTime.UtcNow,
                AFFECTED_USER = affectedUser

            };

            _database.LOGs.Add(currentLog);
            await _database.SaveChangesAsync();
        }

    }
}
