using System.Dynamic;
using System.Runtime.CompilerServices;
using Domain;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ap2ex2.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserService _userService;

        public ChatController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: ChatController
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("id") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View(_userService.GetUser(HttpContext.Session.GetString("id")));
        }
        
        [HttpPost]
        public ActionResult AddContact(string contactToAdd)
        {
            User userToAdd = _userService.GetUser(contactToAdd);
            _userService.AddContacts(HttpContext.Session.GetString("id"), userToAdd.Id);
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult ShowChat(string userId)
        {
            User userChat = _userService.GetUser(userId);
            _userService.GetUser(HttpContext.Session.GetString("id")).UserInChat = userChat;
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult SendMessage(string messageToSend)
        {
            if(messageToSend != "")
            {
                User loggedUser = _userService.GetUser(HttpContext.Session.GetString("id"));
                User userInChat = loggedUser.UserInChat;
                _userService.SendMessage(messageToSend, loggedUser.Id, userInChat.Id);
            }
            return RedirectToAction("Index");
        }
    }
}
