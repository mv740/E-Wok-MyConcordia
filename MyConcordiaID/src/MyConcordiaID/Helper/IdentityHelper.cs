using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
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
