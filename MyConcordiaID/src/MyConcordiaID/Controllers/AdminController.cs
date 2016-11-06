using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Models;
using MyConcordiaID.Models.Admin;
using MyConcordiaID.Models.Student;
using OracleEntityFramework;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {

        private readonly DatabaseEntities _database;
        private IAdminRepository AdminRepo { get; set; }

        public AdminController(IAdminRepository admins, DatabaseEntities context)
        {
            AdminRepo = admins;
            _database = context;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("PicturePeriod")]
        public IActionResult SetPicturePeriod([FromBody] PeriodSetting setting)
        {

            AdminRepo.SetYearUpdatePicturePeriod(setting);

            return Ok();
        }





    }
}
