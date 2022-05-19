using System.Runtime.CompilerServices;
using ap2ex2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ap2ex2.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserService _userService;
        
        public ChatController()
        {
            _userService = new UserService();

        }
        // GET: ChatController
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View(_userService.GetUser(HttpContext.Session.GetInt32("id")));
        }
    }
}
