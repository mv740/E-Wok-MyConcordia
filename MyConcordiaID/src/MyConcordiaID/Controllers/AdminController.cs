using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Helper;
using MyConcordiaID.Models;
using MyConcordiaID.Models.Admin;
using MyConcordiaID.Models.Log;
using System.Security.Claims;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {

        private IAdminRepository _adminRepo { get; set; }
        private ILogRepository _logRepo { get; set; }

        public AdminController(IAdminRepository admins, ILogRepository logs)
        {
            _adminRepo = admins;
            _logRepo = logs;
        }

        /// <summary>
        ///  Set an update picture period for an specific academic year
        /// </summary>
        /// <remarks>Must be authenticated!</remarks>
        /// <param name="setting"></param>
        /// <returns></returns>
        /// <response code="200">Submited new period</response>
        /// <response code="401">Not authenticated</response>
        [HttpPost]
        [Route("PicturePeriod")]
        public IActionResult SetPicturePeriod([FromBody] PeriodSetting setting)
        {
            var authenticatedUser = GetAuthenticatedUserNetname();

            var hasUpdatedPeriodSetting = _adminRepo.SetYearUpdatePicturePeriod(setting);

            _logRepo.LoggerAsync(authenticatedUser,
                hasUpdatedPeriodSetting ? Log.Action.ModifiedPictureUpdatePeriod : Log.Action.CreatePictureUpdatePeriod,
                null);


            return Ok();
        }

        /// <summary>
        ///  Return the current update picture period for this academic year
        /// </summary>
        /// <response code="200">update picture period</response>
        /// <response code="404">No update picture period have been set for this year</response>
        [HttpGet]
        [Route("UpdatePeriod")]
        [ProducesResponseType(typeof(PeriodSetting), 200)]
        public IActionResult GetUpdatePicturePeriod()
        {
            var result = _adminRepo.GetUpdatePicturePeriodAsync().Result;

            if(result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result);
        }

        /// <summary>
        ///  Retrives a specifc update picture period by year
        /// </summary>
        /// <param name="year"></param>
        /// <response code="200">Return picture period</response>
        /// <response code="404">update picture period doesn't exist for that year</response>
        [HttpGet]
        [Route("UpdatePeriod/{year}")]
        [ProducesResponseType(typeof(PeriodSetting), 200)]
        public IActionResult GetUpdatePicturePeriod(int year)
        {
            var result = _adminRepo.GetUpdatePicturePeriodAsync(year).Result;

            if(result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result);
        }

        /// <summary>
        ///  Retrives all update picture periods 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdatePeriods")]
        [ProducesResponseType(typeof(List<PeriodSetting>), 200)]
        public IActionResult GetAllUpdatePicturePeriod()
        {
            var result = _adminRepo.GetAllUpdatePicturePeriodAsync().Result;

            return new ObjectResult(result);
        }

        private string GetAuthenticatedUserNetname()
        {
            var firstName = User.FindFirstValue(ClaimTypes.GivenName);
            var lastName = User.FindFirstValue(ClaimTypes.Surname);
            return StudentHelper.GenerateNetName(firstName, lastName);
        }
    }
}
