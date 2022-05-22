using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

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
            /*string loggedUserId = getLoggedUserId();
            return Enumerable.Select<User, ApiUser>(_service.GetContacts((string) loggedUserId), user => new ApiUser(user)).ToArray();*/
            return Enumerable.Select<User, ApiUser>(_service.GetContacts("user1"), user => new ApiUser(user)).ToArray();
        }

        private string getLoggedUserId()
        {
            var stream = "[encoded jwt]";
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS.Claims.First(claim => claim.Type == "UserId").Value;
        }
    }
}