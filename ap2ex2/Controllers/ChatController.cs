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
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View(_userService.GetUser(HttpContext.Session.GetInt32("id")));
        }
        
        [HttpPost]
        public ActionResult AddContact(string contactToAdd)
        {
            User userToAdd = _userService.GetUserByUsername(contactToAdd);
            _userService.AddContacts(HttpContext.Session.GetInt32("id"), userToAdd.Id);
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult ShowChat(int userId)
        {
            User userChat = _userService.GetUser(userId);
            _userService.GetUser(HttpContext.Session.GetInt32("id")).UserInChat = userChat;
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult SendMessage(string messageToSend)
        {
            User loggedUser = _userService.GetUser(HttpContext.Session.GetInt32("id"));
            Message message = new Message() { sentFrom = loggedUser, sendTo = loggedUser.UserInChat, text = messageToSend };

            List<Message> loggedUserList;
            if (loggedUser.Messages.TryGetValue(loggedUser.UserInChat.Id, out loggedUserList))
            {
                loggedUserList.Add(message);
            }
            else
            {
                loggedUserList = new List<Message>();
                loggedUserList.Add(message);
                loggedUser.Messages.Add(loggedUser.UserInChat.Id, loggedUserList);
            }

            List<Message> userInChatList;
            if (loggedUser.Messages.TryGetValue(loggedUser.Id, out userInChatList))
            {
                userInChatList.Add(message);
            }
            else
            {
                userInChatList = new List<Message>();
                userInChatList.Add(message);
                loggedUser.UserInChat.Messages.Add(loggedUser.Id, userInChatList);
            }


            return RedirectToAction("Index");
        }
    }
}
