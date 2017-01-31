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
    public class EventController : Controller
    {

        private ILogRepository _logRepo { get; set; }


        public EventController(ILogRepository logs)
        {
            _logRepo = logs;
        }



    }
}
