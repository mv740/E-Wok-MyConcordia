using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Log
{
    public interface ILogRepository
    {
        Task LoggerAsync(string netname, Log.Action action, String affectedUser);
        Task<IEnumerable<dynamic>> GetLalestLogsAsync(int count);
        Task<IEnumerable<dynamic>> GetStudentLogsAsync(string netName);
    }
}
