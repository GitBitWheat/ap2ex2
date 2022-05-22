using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace ap2ex2.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }
        
        // GET: UsersController/Index
        public IActionResult AllUsers()
        {
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
                HttpContext.Session.SetString("id", username);
                return RedirectToAction(nameof(Index), "Chat");
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
        public ActionResult Signup([Bind("Id,Name,Password")] User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_service.doesUserExist(user.Id))
            {
                ViewData["usernameAlreadyExists"] = true;
                return View();
            }

            _service.AddUser(user);
            return RedirectToAction(nameof(Login));
        }

        // GET: UsersController/Details
        public ActionResult Details(string id)
        {
            User user = _service.GetUser(id);
            return View(user);
        }
        // private async void SessionSignIn(User account)
        // {
        //     var claims = new List<Claim>() {
        //         new Claim(ClaimTypes.Name, account.Username),
        //     };
        //    
        //     var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //     var authProperties = new AuthenticationProperties
        //     {
        //         //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10);
        //     };
        //     await HttpContext.SignInAsync(
        //         CookieAuthenticationDefaults.AuthenticationScheme,
        //         new ClaimsPrincipal(claimsIdentity),
        //         authProperties);
        // }

        public ActionResult GetContacts()
        {
            string? loggedUserId = HttpContext.Session.GetString("id");
            if (loggedUserId == null)
                return NotFound();
            else
            {
                var contacts = _service.GetContacts((string) loggedUserId);
                return Json(contacts);
            }
        }
    }
}