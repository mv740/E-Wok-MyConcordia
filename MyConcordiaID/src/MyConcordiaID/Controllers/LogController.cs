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
        private ILogRepository LogRepo { get; set; }


        public LogController(ILogRepository logs, DatabaseEntities context)
        {
            LogRepo = logs;
            _database = context;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetLatest50()
        {
            return new ObjectResult(LogRepo.GetLalestLogs(50));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{value}")]
        public IActionResult GetLatestLogs(int value)
        {
            return new ObjectResult(LogRepo.GetLalestLogs(value));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{netname}")]
        public IActionResult GetStudentLogs(string netName)
        {
            return new ObjectResult(LogRepo.GetStudentLogs(netName));
        }

    }
}
