using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Domain;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ap2ex2.Controllers
{
    [Authorize]
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
            User loggedInUser;
            _userService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier), out loggedInUser);
            return View(loggedInUser);
        }
        
        [HttpPost]
        public ActionResult AddContact(string contactToAdd)
        {
            _userService.AddContact(User.FindFirstValue(ClaimTypes.NameIdentifier), new Contact()
            {
                Id = contactToAdd,
                Name = contactToAdd,
                Server = server
            });
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult ShowChat(string userId)
        {
            User loggedInUser, userInChat;
            _userService.GetUser(userId, out userInChat);
            _userService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier), out loggedInUser);

            loggedInUser.UserInChat = userInChat;
            return RedirectToAction("Index");
        }

        /*
        [HttpPost]
        public ActionResult SendMessage(string messageToSend)
        {
            if(messageToSend != "")
            {
                User loggedUser = _userService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User userInChat = loggedUser.UserInChat;
                _userService.SendMessage(messageToSend, loggedUser.Id, userInChat.Id);
            }
            return RedirectToAction("Index");
        }
        */
    }
}
