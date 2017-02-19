using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Models.Log;

namespace MyConcordiaID.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class LogController : Controller
    {

        private ILogRepository _logRepo { get; set; }


        public LogController(ILogRepository logs)
        {
            _logRepo = logs;
        }

        /// <summary>
        ///  Retrieves the latest 50 logs 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetLatest50()
        {
            var logs = _logRepo.GetLalestLogsAsync(50);

            return new ObjectResult(logs.Result);
        }

        /// <summary>
        ///  Retrieve the latest 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{value:int}")]
        public IActionResult GetLatestLogs(int value)
        {
            var logs = _logRepo.GetLalestLogsAsync(value);

            return new ObjectResult(logs.Result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{netname}")]
        public IActionResult GetStudentLogs(string netName)
        {
            var logs = _logRepo.GetStudentLogsAsync(netName);

            return new ObjectResult(logs.Result);
        }

    }
}
