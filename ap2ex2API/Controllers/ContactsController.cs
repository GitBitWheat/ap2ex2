using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;

namespace ap2ex2API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IUserService _service;

        public ContactsController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<ApiUser> Index()
        {
            string loggedUserId = getLoggedUserId();
            return Enumerable.Select<User, ApiUser>(_service.GetContacts(loggedUserId), user => new ApiUser(user)).ToArray();
        }

        [HttpGet("{id}")]
        public ApiUser? GetContactWithId(string id)
        {
            string loggedUserId = getLoggedUserId();
            if (_service.isContactOfUser(loggedUserId, id))
                return new ApiUser(_service.GetUser(id));
            else
                return null;
        }

        /*
         * This line doesn't work:
        [HttpGet("{id}", Name = "messages")]
         * The function itself does work:
        public Message[]? GetMessagesWithContactOfId(string id)
        {
            string loggedUserId = getLoggedUserId();
            List<Message> messagesList = _service.getMessagesBetweenTwoUsers(loggedUserId, id);
            if (null == messagesList)
                return null;
            else
                return messagesList.ToArray();
        }
        */

        private string getLoggedUserId()
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(_bearer_token);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS.Claims.First(claim => claim.Type == "UserId").Value;
        }
    }
}