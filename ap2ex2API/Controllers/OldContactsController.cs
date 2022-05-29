using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;
using ap2ex2API.Models;

namespace ap2ex2API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OldContactsController : ControllerBase
    {
        private readonly IUserService _service;

        public OldContactsController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<UserApiModel> GetAllContacts()
        {
            string loggedUserId = GetLoggedUserId();
            List<User> contactList;
            _service.GetContacts(loggedUserId, out contactList);

            return Enumerable.Select(contactList, user => new UserApiModel(user)).ToArray();
        }

        [HttpPost]
        public IActionResult AddContact([FromBody] UserApiModel newContactApiModel)
        {
            string loggedUserId = GetLoggedUserId();

            if (newContactApiModel.Id.Equals(loggedUserId))
                return Forbid();

            User newContact;
            if (!_service.GetUser(newContactApiModel.Id, out newContact))
            {
                newContact = newContactApiModel.convertToUser();
                _service.AddUser(newContact);
            }

            if (!_service.AddContacts(loggedUserId, newContact.Id))
                return Forbid();

            return CreatedAtAction(nameof(AddContact), new { Id = newContactApiModel.Id });
        }

        [HttpGet("{id}")]
        public IActionResult GetContactWithId(string id)
        {
            string loggedUserId = GetLoggedUserId();
            if (!_service.IsContactOfUser(loggedUserId, id))
                return NotFound();

            User contact;
            _service.GetUser(id, out contact);
            return Ok(new UserApiModel(contact));
        }

        [HttpPut("{id}")]
        public IActionResult ChangeContactDetails([FromBody] ContactDetailsModel userDetails, string id)
        {
            string loggedUserId = GetLoggedUserId();
            if (!_service.IsContactOfUser(loggedUserId, id))
                return NotFound();

            User contact;
            _service.GetUser(id, out contact);
            contact.Name = userDetails.Name;
            contact.Server = userDetails.Server;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveContact(string id)
        {
            string loggedUserId = GetLoggedUserId();
            if (_service.RemoveContacts(loggedUserId, id))
                return NoContent();
            else
                return NotFound();
        }

        [HttpGet("{id}/messages")]
        public IActionResult GetMessagesWithContactOfId(string id)
        {
            string loggedUserId = GetLoggedUserId();
            List<Message> msgList;
            if (_service.GetMessagesBetweenTwoUsers(loggedUserId, id, out msgList))
                return Ok(Enumerable.Select(msgList, message => new MessageApiModel(message, loggedUserId)).ToArray());
            else
                return NotFound();
        }

        [HttpPost("{id}/messages")]
        public IActionResult SendMessage([FromBody] MessageContentModel messageContent, string id)
        {
            string loggedUserId = GetLoggedUserId();
            _service.SendMessage(messageContent.Content, loggedUserId, id);
            return CreatedAtAction(nameof(GetMessagesWithContactOfId), new { Content = messageContent.Content });
        }

        [HttpGet("{id}/messages/{id2}")]
        public IActionResult GetMessageOfIdWithContactOfId(string id, int id2)
        {
            string loggedUserId = GetLoggedUserId();
            Message message;
            if (_service.GetMessageOfIdBetweenTwoUsers(loggedUserId, id, id2, out message))
                return Ok(new MessageApiModel(message, loggedUserId));
            else
                return NotFound();
        }

        [HttpPut("{id}/messages/{id2}")]
        public IActionResult ChangeMessageDetails([FromBody] MessageContentModel messageContent, string id, int id2)
        {
            string loggedUserId = GetLoggedUserId();
            Message message;
            if (!_service.GetMessageOfIdBetweenTwoUsers(loggedUserId, id, id2, out message))
                return NotFound();

            message.Content = messageContent.Content;
            return NoContent();
        }

        [HttpDelete("{id}/messages/{id2}")]
        public IActionResult RemoveMessage(string id, int id2)
        {
            string loggedUserId = GetLoggedUserId();
            if (_service.RemoveMessage(loggedUserId, id, id2))
                return NoContent();
            else
                return NotFound();
        }

        private string GetLoggedUserId()
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(_bearer_token);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS.Claims.First(claim => claim.Type == "UserId").Value;
        }
    }
}