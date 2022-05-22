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
                User _loggedUser = _userService.GetUser(HttpContext.Session.GetString("id"));
                User _userInChat = _loggedUser.UserInChat;

                if (!_loggedUser.MessagesD.ContainsKey(_userInChat.Id))
                {
                    _loggedUser.MessagesD.Add(_userInChat.Id, new List<Message>());
                    _userInChat.MessagesD.Add(_loggedUser.Id, new List<Message>());
                }

                _loggedUser.MessagesD[_userInChat.Id].Add(new Message()
                {
                    sentFrom = _loggedUser, sendTo = _userInChat,
                    text = messageToSend, dateTime = DateTime.Now
                });

                _userInChat.MessagesD[_loggedUser.Id].Add(new Message()
                {
                    sentFrom = _loggedUser, sendTo = _userInChat,
                    text = messageToSend, dateTime = DateTime.Now
                });
            }
            return RedirectToAction("Index");
        }
    }
}
