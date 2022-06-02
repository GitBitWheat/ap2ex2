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
    public class ContactsController : ControllerBase
    {
        private readonly IUserService _service;

        public ContactsController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<ContactApiModel> GetAllContacts()
        {
            string loggedUserId = GetLoggedUserId();
            List<Contact> contactList;
            _service.GetContacts(loggedUserId, out contactList);
            return Enumerable.Select(contactList, c => new ContactApiModel(c)).ToArray();
        }

        [HttpPost]
        public IActionResult AddContact([FromBody] NewContactModel newContact)
        {
            string loggedUserId = GetLoggedUserId();

            if (!_service.AddContact(loggedUserId, newContact.ConvertToContact()))
                return Forbid();

            return CreatedAtAction(nameof(AddContact), new { Id = newContact.Id });
        }

        [HttpGet("{id}")]
        public IActionResult GetContactWithId(string id)
        {
            string loggedUserId = GetLoggedUserId();
            Contact contact;

            if (!_service.GetContactOfId(loggedUserId, id, out contact))
                return NotFound();

            return Ok(new ContactApiModel(contact));
        }

        [HttpPut("{id}")]
        public IActionResult ChangeContactDetails([FromBody] ContactDetailsModel contactDetails, string id)
        {
            string loggedUserId = GetLoggedUserId();

            if (_service.UpdateContactOfId(loggedUserId, id, contactDetails.Name, contactDetails.Server))
                return NoContent();
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveContact(string id)
        {
            string loggedUserId = GetLoggedUserId();

            if (_service.RemoveContact(loggedUserId, id))
                return NoContent();
            else
                return NotFound();
        }

        [HttpGet("{id}/messages")]
        public IActionResult GetMessagesWithContactOfId(string id)
        {
            string loggedUserId = GetLoggedUserId();
            List<Message> msgList;

            if (_service.GetMessagesBetweenUserAndContact(loggedUserId, id, out msgList))
                return Ok(Enumerable.Select(msgList, message => new MessageApiModel(message, loggedUserId)).ToArray());
            else
                return NotFound();
        }

        [HttpPost("{id}/messages")]
        public IActionResult SendMessage([FromBody] MessageContentModel messageContent, string id)
        {
            string loggedUserId = GetLoggedUserId();

            if (_service.SendMessage(messageContent.Content, loggedUserId, id))
                return CreatedAtAction(nameof(SendMessage), new { Content = messageContent.Content });
            else
                return Forbid();
        }

        [HttpGet("{id}/messages/{id2}")]
        public IActionResult GetMessageOfIdWithContactOfId(string id, int id2)
        {
            string loggedUserId = GetLoggedUserId();
            Message message;

            if (_service.GetMessageOfIdBetweenUserAndContact(loggedUserId, id, id2, out message))
                return Ok(new MessageApiModel(message, loggedUserId));
            else
                return NotFound();
        }

        [HttpPut("{id}/messages/{id2}")]
        public IActionResult ChangeMessageDetails([FromBody] MessageContentModel messageContent, string id, int id2)
        {
            string loggedUserId = GetLoggedUserId();

            if (_service.UpdateMessageOfIdBetweenUserAndContact(loggedUserId, id, id2, messageContent.Content))
                return NoContent();
            else
                return NotFound();
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