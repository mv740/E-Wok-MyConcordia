using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Log
{
    public interface ILogRepository
    {
        void Logger(string netname, Log.Action action, String affectedUser);
        Task<IEnumerable<dynamic>> GetLalestLogs(int count);
        Task<IEnumerable<dynamic>> GetStudentLogs(string netName);
    }
}
