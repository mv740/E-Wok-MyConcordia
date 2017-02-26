using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Helper;
using MyConcordiaID.Models.Graduation;
using System.Security.Claims;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class GraduationController : Controller
    {
        private IGraduationRepository _gradRepo { get; set; }

        public GraduationController(IGraduationRepository graduation)
        {
            _gradRepo = graduation;
        }

        /// <summary>
        ///  Get Marshalling card if your graduation status is positive
        /// </summary>
        /// <returns></returns>
        /// <response code="200">return Marshalling card</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        public IActionResult GetMarshallingCard()
        {
            // var netName = getAuthenticatedUserNetname();

            return new ObjectResult(_gradRepo.GetMarshallingCard(null));
        }


        /// <summary>
        /// Debug graduation status false
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Denied card</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("test")]
        public IActionResult GetMarshallingCardRequestDenied()
        {
            // var netName = getAuthenticatedUserNetname();

            return new ObjectResult(_gradRepo.GetMarshallingCardRequestDenied(null));
        }

        private string GetAuthenticatedUserNetname()
        {
            var firstName = User.FindFirstValue(ClaimTypes.GivenName);
            var lastName = User.FindFirstValue(ClaimTypes.Surname);
            return StudentHelper.GenerateNetName(firstName, lastName);
        }
    }
}
