using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;
using ap2ex2.Models;

namespace ap2ex2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationsController : ControllerBase
    {
        private readonly IUserService _userService;

        public InvitationsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult HandleInvitation([FromBody] InvitationModel invitationModel)
        {
            if (_userService.DoesUserExist(invitationModel.To))
            {
                Contact newContact = new Contact()
                {
                    Id = invitationModel.From,
                    Name = invitationModel.From,
                    Server = invitationModel.Server
                };
                if (_userService.AddContact(invitationModel.To, newContact))
                    return CreatedAtAction(nameof(HandleInvitation), new { Id = invitationModel.From });
                else
                    return Forbid();
            }
            else
                return NotFound();
        }

    }
}
