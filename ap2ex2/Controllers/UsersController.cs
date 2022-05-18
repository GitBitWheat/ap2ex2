using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ap2ex2.Services;
using ap2ex2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace ap2ex2.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _service;

        /* This is the constructor that should be used but I'm not sure how to do it
        public UsersController(IUserService service)
        {
            _service = service;
        }
        */
        public UsersController()
        {
            _service = new UserService();
        }

        // GET: UsersController/Index
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
                return RedirectToAction("Login", "users");
            return View(_service.GetAllUsers());
        }

        // GET: UsersController/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: UsersController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (_service.Login(username, password))
            {
                Signin(username);
                HttpContext.Session.SetString("username", username);
                return RedirectToAction(nameof(Index), "Users");
            }
            else
            {
                ViewData["Error"] = "Username or password is incorrect.";
            }

            return View();
        }


        // GET: UsersController/Signup
        public ActionResult Signup()
        {
            return View();
        }

        // POST: UsersController/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup([Bind("Username,Nickname,Password,Pfp")] User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            /*
             * Checking if there already exists a user with this username...
             */
            int id = _service.AddUser(user);
            try
            {
                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Details
        public ActionResult Details(int id)
        {
            return View(_service.GetUser(id));
        }
        private async void Signin(string acountUsername)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, acountUsername),

            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10);

            };
            await HttpContent.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        }
    }
}