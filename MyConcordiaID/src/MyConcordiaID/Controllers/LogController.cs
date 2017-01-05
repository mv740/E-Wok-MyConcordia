using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Models.Log;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Controllers
{   

    [Authorize]
    [Route("api/[controller]")]
    public class LogController : Controller
    {

        private readonly DatabaseEntities _database;
        private ILogRepository _logRepo { get; set; }


        public LogController(ILogRepository logs, DatabaseEntities context)
        {
            _logRepo = logs;
            _database = context;
        }

        /// <summary>
        ///  Retrieves the latest 50 logs 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetLatest50()
        {
            return new ObjectResult(_logRepo.GetLalestLogs(50));
        }

        /// <summary>
        ///  Retrieve the latest 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{value}")]
        public IActionResult GetLatestLogs(int value)
        {
            return new ObjectResult(_logRepo.GetLalestLogs(value));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{netname}")]
        public IActionResult GetStudentLogs(string netName)
        {
            return new ObjectResult(_logRepo.GetStudentLogs(netName));
        }

    }
}
