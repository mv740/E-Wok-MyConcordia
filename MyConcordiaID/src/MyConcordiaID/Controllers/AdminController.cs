using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Helper;
using MyConcordiaID.Models;
using MyConcordiaID.Models.Admin;
using MyConcordiaID.Models.Log;
using OracleEntityFramework;
using System.Security.Claims;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {

        private readonly DatabaseEntities _database;
        private IAdminRepository _adminRepo { get; set; }
        private ILogRepository _logRepo { get; set; }

        public AdminController(IAdminRepository admins, ILogRepository logs, DatabaseEntities context)
        {
            _adminRepo = admins;
            _database = context;
            _logRepo = logs;
        }

   
        [Authorize]
        [HttpPost]
        [Route("PicturePeriod")]
        public IActionResult SetPicturePeriod([FromBody] PeriodSetting setting)
        {
            var authenticatedUser = getAuthenticatedUserNetname();

            var hasUpdatedPeriodSetting = _adminRepo.SetYearUpdatePicturePeriod(setting);
            if (hasUpdatedPeriodSetting)
            {
                _logRepo.Logger(authenticatedUser, Log.Action.ModifiedPictureUpdatePeriod, null);
            }
            else
            {
                _logRepo.Logger(authenticatedUser, Log.Action.CreatePictureUpdatePeriod, null);
            }


            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UpdatePeriod")]
        public IActionResult GetUpdatePicturePeriod()
        {
            var result = _adminRepo.GetUpdatePicturePeriod();

            if(result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UpdatePeriod/{year}")]
        public IActionResult GetUpdatePicturePeriod(int year)
        {
            var result = _adminRepo.GetUpdatePicturePeriod(year);

            if(result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UpdatePeriods")]
        public IActionResult GetAllUpdatePicturePeriod()
        {
            var result = _adminRepo.GetAllUpdatePicturePeriod();

            return new ObjectResult(result);
        }

        public string getAuthenticatedUserNetname()
        {
            var firstName = User.FindFirstValue(ClaimTypes.GivenName);
            var lastName = User.FindFirstValue(ClaimTypes.Surname);
            return StudentHelper.GenerateNetName(firstName, lastName);
        }



    }
}
