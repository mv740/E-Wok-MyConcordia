using System.Collections.Generic;

namespace MyConcordiaID.Models.Log
{
    public interface ILogRepository
    {
        void Logger(string netname, Log.Action action);
        IEnumerable<dynamic> GetLalestLogs(int count);
    }
}
