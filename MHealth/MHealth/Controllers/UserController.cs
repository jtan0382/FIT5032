using MHealth.Models.DTO;
using MHealth.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MHealth.Models.Domain;
using Microsoft.AspNetCore.Identity;


namespace MHealth.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAuthenticationService _service;
        private readonly ILogger _logger;
        private readonly SignInManager<UserModel> _signInManager;

        public UserController(IUserAuthenticationService _service, ILogger<UserModel> _logger, SignInManager<UserModel> _signInManager)
        {
            this._service = _service;
            this._logger = _logger;
            this._signInManager = _signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Role = "user";
            var result = await _service.RegistrationAsync(model);
            //TempData["msg"] = result.StatusMessage;
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["msg"] = result.StatusMessage;
                //TempData["name"] = User.Identity.Name;
                return RedirectToAction(nameof(Signup));

            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.LoginAsync(model);
            //TempData["msg"] = result.StatusMessage;
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["msg"] = result.StatusMessage;
                //TempData["name"] = User.Identity.Name;
                return RedirectToAction(nameof(Login));
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GoogleLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("GoogleCallback", "User",
                                    new { ReturnUrl = returnUrl });

            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            var result = await _service.GoogleCallback(returnUrl, remoteError);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["msg"] = result.StatusMessage;
                //TempData["name"] = User.Identity.Name;
                return RedirectToAction(nameof(Login));
            }

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            TempData.Remove("msg");
            return RedirectToAction("Index", "Home");
        }


        
    }
}
