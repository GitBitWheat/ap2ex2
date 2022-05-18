using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ap2ex2.Services;
using ap2ex2.Models;

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
                return RedirectToAction(nameof(Index), "User");
            }
            else
            {
                ViewData["Error"] = "User or password is incorrect.";
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
            if (!ModelState.IsValid) {
                return View();
            }
            /*
             * Checking if there already exists a user with this username...
             */ 
            int id = _service.AddUser(user);
            try
            {
                return RedirectToAction(nameof(Details), new { id = id });
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
    }
}