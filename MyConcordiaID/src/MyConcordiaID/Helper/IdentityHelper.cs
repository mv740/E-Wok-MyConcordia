using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyConcordiaID.Helper
{
    public class IdentityHelper
    {
        /// <summary>
        /// http://stackoverflow.com/questions/38557942/mocking-iprincipal-in-asp-net-core
        /// </summary>
        /// <param name="givenName"></param>
        /// <param name="surname"></param>
        /// <param name="controller"></param>
        public static void SetUser(string givenName, string surname, Controller controller)
        {
            //mock  user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.GivenName, givenName),
                new Claim(ClaimTypes.Surname, surname)
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

        }

    }
}
