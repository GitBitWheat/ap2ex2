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

        public IActionResult Login()
        {
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
        public ActionResult Signup([Bind("Username,Name,Password")] User user)
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

            SignIn(user.Id);
            _service.AddUser(user);
            return RedirectToAction(nameof(Login));
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (_service.Login(username, password))
            {
                SignIn(username.ToLower());
                return RedirectToAction(nameof(Index), "Chat");
            }
            else
            {
                ViewData["Error"] = "Username or password is incorrect.";
            }

            return View();
        }

        // GET: UsersController/Details
        public ActionResult Details(string id)
        {
            User user = _service.GetUser(id);
            return View(user);
        }
  
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    
        private async void SignIn(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProp = new AuthenticationProperties
            {
            

            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProp);
        }
        

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