using System;
using System.Collections.Generic;

namespace MyConcordiaID.Models.Log
{
    public interface ILogRepository
    {
        void Logger(string netname, Log.Action action, String affectedUser);
        IEnumerable<dynamic> GetLalestLogs(int count);

        IEnumerable<dynamic> GetStudentLogs(string netName);
    }
}
