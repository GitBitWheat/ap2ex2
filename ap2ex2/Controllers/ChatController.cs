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

            if (TempData["contactInChatId"] != null)
                ViewBag.contactInChatId = TempData["contactInChatId"];

            if (TempData["contactInChatName"] != null)
                ViewBag.contactInChatName = TempData["contactInChatName"];

            if (TempData["contactInChatServer"] != null)
                ViewBag.contactInChatServer = TempData["contactInChatServer"];

            return View(loggedInUser);
        }
        
        [HttpPost]
        public ActionResult AddContact(string contactId, string contactName, string contactServer)
        {
            _userService.AddContact(User.FindFirstValue(ClaimTypes.NameIdentifier), new Contact()
            {
                Id = contactId,
                Name = contactName,
                Server = contactServer
            });
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult ShowChat(string contactId)
        {
            Contact contact;
            if (_userService.GetContactOfId(User.FindFirstValue(ClaimTypes.NameIdentifier), contactId, out contact))
            {
                TempData["contactInChatId"] = contact.Id;
                TempData["contactInChatName"] = contact.Name;
                TempData["contactInChatServer"] = contact.Server;
            }

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
