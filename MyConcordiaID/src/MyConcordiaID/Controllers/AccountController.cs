using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyConcordiaID.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using MyConcordiaID.Models.AccountViewModels;
using Microsoft.AspNetCore.Http;
using OracleEntityFramework;
using MyConcordiaID.Helper;
using Microsoft.AspNetCore.Authentication;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyConcordiaID.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly DatabaseEntities _database;
        private readonly ILogger _logger;

        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           ILoggerFactory loggerFactory,
           DatabaseEntities dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _database = dbContext;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        //
        // GET: /Account/ExternalLogin
        [HttpGet]
        [AllowAnonymous]
        [Route("Account/ExternalLogin")]
        public IActionResult GetExternalLogin(string provider, string returnUrl = null)
        {
            
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginMobileCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET : /Account/ExternalLogins
        [HttpGet]
        [AllowAnonymous]
        [Route("Account/ExternalLogins")]
        public ExternalLoginViewModel GetExternalLogins(string returnUrl, bool generateState = false)
        {

            ExternalLoginViewModel login;
            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            ///todo : replace client_id literal string by fetching directly from .json 
            login = new ExternalLoginViewModel
            {
                Name = Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme,
                Url = Url.Action("ExternalLogin", new
                {
                    provider = "Google",
                    response_type = "token",
                    client_id = "410122942772-7ugs0v8ltf2127of481ie7oej4q5or4j.apps.googleusercontent.com",
                    redirect_uri = new Uri(HttpRequestExtensions.ToUri(Request), returnUrl).AbsoluteUri,
                    state = state
                }),
                State = state

            };

            return login;
        }


        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            
           
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);



            if (result.Succeeded)
            {
                
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
                {
                    Email = email,
                });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginMobileCallback(string returnUrl = null, string remoteError = null)
        {


            if (remoteError != null)
            {
                //Error from external provider
                return new BadRequestResult();
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                //need to login
                return new UnauthorizedResult();
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
           
                var authToken = info.AuthenticationTokens;
                authToken.GetEnumerator().MoveNext();
                AuthenticationToken token = authToken.GetEnumerator().Current;
                Authentication authObject = new Authentication() { AuthToken = token.Value };
                return new OkObjectResult(authObject);

            }
            else
            {
                return Unauthorized();
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user); // see how to use move this to oracle db

                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastname = info.Principal.FindFirstValue(ClaimTypes.Surname);
             
                
                //todo change primary key to id

                STUDENT newStudent = new STUDENT
                {
                    NETNAME = StudentHelper.GenerateNetName(firstName,lastname),
                    ID = StudentHelper.GenerateRandomId(),
                    FIRSTNAME = firstName,
                    LASTNAME = lastname,
                    DOB = DateTime.UtcNow,
                    UGRADSTATUS = "U"
                };
                _database.STUDENTS.Add(newStudent);
                _database.SaveChanges();


                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion


    }
}
